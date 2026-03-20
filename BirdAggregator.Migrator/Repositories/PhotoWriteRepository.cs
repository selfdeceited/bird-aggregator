using System.Globalization;
using System.Text.Json;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using BirdAggregator.Infrastructure.Mongo;
using BirdAggregator.Migrator.ResponseModels;
using Colorify;
using MongoDB.Driver;

namespace BirdAggregator.Migrator.Repositories;

public class PhotoWriteRepository(IMongoConnection mongoConnection) : IPhotoWriteRepository
{
    private IMongoCollection<PhotoModel> Photos => mongoConnection.Database
        .GetCollection<PhotoModel>("photos")
        .WithWriteConcern(WriteConcern.WMajority);

    private IMongoCollection<BirdModel> Birds => mongoConnection.Database
        .GetCollection<BirdModel>("birds")
    .WithWriteConcern(WriteConcern.WMajority);

    public async Task SavePhotos(IList<SavePhotoModel> savePhotoModels, CancellationToken ct)
    {
        if (!savePhotoModels.Any()) return;

        var birdNames = savePhotoModels
            .Select(x => x.photo.title._content)
            .SelectMany(ExtractBirdNames);

        var batchHash = savePhotoModels
            .Select(x => x.photo.id)
            .Aggregate(0, (current, t) => current ^ t.GetHashCode())
            .ToString();

        Program.ColoredConsole.WriteLine($"            > #{batchHash} for ids: ${string.Join(", ", savePhotoModels.Select(x => x.photo.id))}", Colors.txtMuted);
        try
        {
            var result = await mongoConnection.ExecuteInTransaction(async (s, cancellationToken) =>
            {
                var birdsInBatch = await GetOrUpdateBirds(birdNames, s, batchHash, cancellationToken);

                IEnumerable<BirdModel> BirdsForModel(SavePhotoModel _)
                {
                    var birdNamesForModel = ExtractBirdNames(_.photo.title._content)
                        .Where(name => name != "undefined")
                        .Select(x => ToBirdModel(x).Name)
                        .Distinct()
                        .ToArray();
                    return birdsInBatch.Where(b => birdNamesForModel.Contains(b.Name));
                }

                var photoModels = savePhotoModels.Select(_ => ToPhotoModel(_.photo, _.sizes, _.location, BirdsForModel(_)));

                // todo: fix duplicate entries - upsert by flickr id?
                await Photos.InsertManyAsync(s, photoModels, new InsertManyOptions { IsOrdered = false },
                    cancellationToken);

                return 1; // todo: return duplicates to schedule its fixes
            }, ct);
        }
        catch (Exception e)
        {
            Program.ColoredConsole.WriteLine($"{e.Message}\n{e.StackTrace}", Colors.txtWarning);
            Program.ColoredConsole.WriteLine($"#{batchHash}: insert has failed", Colors.bgWarning);
            Program.ColoredConsole.WriteLine(JsonSerializer.Serialize(savePhotoModels), Colors.txtMuted);
            throw;
        }
    }

    private async Task<IEnumerable<BirdModel>> GetOrUpdateBirds(IEnumerable<string> birdNames, IClientSessionHandle clientSessionHandle, string batchId, CancellationToken ct)
    {
        var inputBirdModels = birdNames
            .Where(name => name != "undefined")
            .Distinct()
            .Select(ToBirdModel)
            .ToArray();

        Program.ColoredConsole.WriteLine($"            > # #{batchId} local: ${string.Join(", ", inputBirdModels.Select(x => $"{x.Name}"))}", Colors.txtMuted);
        var birdsFromDb = (await GetBirdsFromDb(clientSessionHandle, inputBirdModels, ct)).ToArray();
        Program.ColoredConsole.WriteLine($"            > # #{batchId} db: ${string.Join(", ", birdsFromDb.Select(x => $"{x.dbModel?.Name} {x.dbModel?.Id}"))}", Colors.txtMuted);
        var toInsert = birdsFromDb
            .Where(x => x.dbModel == null)
            .Select(x => x.inputModel)
            .ToArray();

        if (toInsert.Length == 0)
        {
            Program.ColoredConsole.WriteLine($"            > #{batchId} nothing to insert", Colors.txtMuted);
            return birdsFromDb.Select(x => x.dbModel).ToList();
        }

        await Birds.InsertManyAsync(clientSessionHandle, toInsert, cancellationToken: ct);

        var updatedModels = (await GetBirdsFromDb(clientSessionHandle, inputBirdModels, ct)).ToArray();
        Program.ColoredConsole.WriteLine($"            > #{batchId} updated: ${string.Join(", ", updatedModels.Select(x => $"{x.dbModel?.Name} {x.dbModel?.Id}"))}", Colors.txtMuted);

        var dbModels = updatedModels.Select(x => x.dbModel).ToList();
        if (dbModels.Any(x => x == null))
        {
            throw new Exception("some models are not saved to db");
        }

        return dbModels;
    }

    private static PhotoModel ToPhotoModel(PhotoResponse.Photo photo, Sizes sizes, Location location, IEnumerable<BirdModel> birds)
    {
        try
        {
            var largestSize = sizes.size.MaxBy(x => x.height);
            return new PhotoModel
            {
                Description = photo.description._content,
                BirdIds = birds.Select(x => x.Id),
                Flickr = new FlickrModel
                {
                    FarmId = photo.farm,
                    Id = photo.id,
                    ServerId = photo.server,
                    Secret = photo.originalsecret
                },
                Ratio = largestSize != null ? (double)largestSize.width / largestSize.height : 1,
                Location = location == null
                    ? null
                    : new LocationModel
                    {
                        Country = location.country._content,
                        Neighbourhood = location.neighbourhood._content,
                        Region = location.region._content,
                        X = double.Parse(location.longitude, CultureInfo.InvariantCulture),
                        Y = double.Parse(location.latitude, CultureInfo.InvariantCulture),
                        GeoTag = location.place_id,
                        Locality = location.locality?._content
                    },
                DateTaken = DateTime.Parse(photo.dates.taken),
            };
        }
        catch (Exception)
        {
            File.AppendAllText("out.txt",
                JsonSerializer.Serialize(photo)
                + "\n"
                + JsonSerializer.Serialize(location)
                + "\n"
            );
            throw;
        }
    }


    private async Task<IEnumerable<(BirdModel dbModel, BirdModel inputModel)>> GetBirdsFromDb(
        IClientSessionHandle clientSessionHandle, BirdModel[] birdModels, CancellationToken ct)
    {
        var names = birdModels.Select(x => x.Name).ToArray();
        var birdsToSearch = await Birds.FindAsync(clientSessionHandle, x => names.Contains(x.Name), cancellationToken: ct);
        var birdsDbList = await birdsToSearch.ToListAsync(ct);

        var duplicateEntries = birdsDbList.GroupBy(x => x.Name).Where(x => x.Count() > 1).ToArray();
        if (duplicateEntries.Length != 0)
        {
            throw new Exception($"duplicate entry exists: {JsonSerializer.Serialize(duplicateEntries)}");
        }

        var mergedList = birdModels.Select(x => (
            birdsDbList
                .OrderByDescending(m => m.Id.CreationTime)
                .SingleOrDefault(dm => dm.Name == x.Name), x));
        return mergedList;
    }

    private IEnumerable<string> ExtractBirdNames(string title)
    {
        if (title.StartsWith("B: "))
            title = title["B: ".Length..];
        return title.Contains(", ") ? title.Split(", ").Distinct() : new[] { title };
    }

    private BirdModel ToBirdModel(string name)
    {
        Func<string, int> indexOf = name.IndexOf;

        return new BirdModel
        {
            Name = name[..(indexOf("(") - 1)],
            Latin = name.Substring(indexOf("(") + 1, indexOf(")") - indexOf("(") - 1)
        };
    }

    public async Task TrackDuplicatePhotos(CancellationToken ct)
    {
        Program.ColoredConsole.WriteLine("Tracking duplicates...", Colors.bgWarning);
        try
        {
            await FixDuplicateBirds(ct);
            await FixDuplicatePhotos(ct);
        }
        catch (Exception e)
        {
            Program.ColoredConsole.WriteLine($"{e.Message}\n{e.StackTrace}", Colors.bgDanger);
            throw;
        }

    }

    private async Task FixDuplicateBirds(CancellationToken ct)
    {
        var duplicateBirdsGroups = await Birds
            .Aggregate()
            .Group(x => x.Name, _ => new
            {
                ids = _.Select(b => b.Id),
                count = _.Count()
            })
            .Match(x => x.count > 1)
            .Project(x => new { x.ids })
            .ToListAsync(ct);

        foreach (var group in duplicateBirdsGroups)
        {
            if (group.ids.Count() < 2)
                throw new Exception("wat?!");

            var firstId = group.ids.FirstOrDefault();
            var others = group.ids.Except([firstId]).ToArray();
            foreach (var other in others)
            {
                var findCursor = await Photos.FindAsync(x => x.BirdIds.Contains(other), cancellationToken: ct);
                var photosOfOtherIds = await findCursor.ToListAsync(ct);
                foreach (var photo in photosOfOtherIds)
                {
                    var updateNameDefinition = Builders<PhotoModel>.Update
                        .Set(u => u.BirdIds, photo.BirdIds
                        .Except([other])
                        .Union([firstId]));

                    await Photos.UpdateManyAsync(
                        x => x.Id == photo.Id, updateNameDefinition, cancellationToken: ct);
                }
            }
            await Birds.DeleteManyAsync(x => others.Contains(x.Id), cancellationToken: ct);
            Program.ColoredConsole.WriteLine($"Birds with ids {JsonSerializer.Serialize(others)} removed as duplicates", Colors.bgWarning);
        }
    }
    private async Task FixDuplicatePhotos(CancellationToken ct)
    {
        var duplicatePhotos = await Photos
           .Aggregate()
           .Group(x => x.Flickr.Id, _ => new
           {
               ids = _.Select(b => b.Id),
               count = _.Count()
           })
           .Match(x => x.count > 1)
           .Project(x => new { x.ids })
           .ToListAsync(ct);

        Program.ColoredConsole.WriteLine($"duplicatePhotos: {JsonSerializer.Serialize(duplicatePhotos)}", Colors.bgWarning);

        foreach (var group in duplicatePhotos)
        {
            if (group.ids.Count() < 2)
                throw new Exception("wat?!");

            var firstId = group.ids.FirstOrDefault();
            var others = group.ids.Except([firstId]);
            await Photos.DeleteManyAsync(x => others.Contains(x.Id), ct);
            Program.ColoredConsole.WriteLine($"Photos with ids {JsonSerializer.Serialize(others)} removed as duplicates", Colors.bgWarning);
        }
    }
}
