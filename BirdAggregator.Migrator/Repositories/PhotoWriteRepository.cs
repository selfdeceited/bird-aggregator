using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using BirdAggregator.Infrastructure.Mongo;
using BirdAggregator.Migrator.ResponseModels;
using Colorify;
using MongoDB.Driver;

namespace BirdAggregator.Migrator.Repositories
{
    public class PhotoWriteRepository : IPhotoWriteRepository
    {
        private readonly IMongoConnection _mongoConnection;

        private IMongoCollection<PhotoModel> _photos => _mongoConnection.Database
            .GetCollection<PhotoModel>("photos");
            //.WithWriteConcern(WriteConcern.WMajority);
            //.WithReadConcern(ReadConcern.Linearizable);

            private IMongoCollection<BirdModel> _birds => _mongoConnection.Database
                .GetCollection<BirdModel>("birds");
            //.WithWriteConcern(WriteConcern.WMajority);
            //.WithReadConcern(ReadConcern.Linearizable);

        public PhotoWriteRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }
        
        public async Task SavePhoto(SavePhotoModel savePhotoModel, CancellationToken ct)
        {
            try
            {
                var result = await _mongoConnection.ExecuteInTransaction(async (s, cancellationToken) =>
                {
                    var (photo, location, sizes) = savePhotoModel;
                    var birds = await GetOrUpdateBirds(photo.title._content, photo.id, s, cancellationToken);
                    var photoModel = ToPhotoModel(photo, sizes, location, birds);
                    await _photos.InsertOneAsync(s, photoModel, new InsertOneOptions(), cancellationToken);
                    return 1;
                }, ct);
            }
            catch (Exception e)
            {
                Program.ColoredConsole.WriteLine($"{e.Message}\n{e.StackTrace}", Colors.txtWarning);
                Program.ColoredConsole.WriteLine($"retry for saving #{savePhotoModel.photo.id} is scheduled", Colors.bgWarning);
                throw;
            }
        }

        private PhotoModel ToPhotoModel(PhotoResponse.Photo photo, Sizes sizes, Location location, IEnumerable<BirdModel> birds)
        {
            var largestSize = sizes.size.OrderByDescending(x => x.height).FirstOrDefault();
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
                Ratio = largestSize != null ? (double) largestSize.width / largestSize.height : 1,
                Location = location == null
                    ? null
                    : new LocationModel
                    {
                        Country = location.country._content,
                        Neighbourhood = location.neighbourhood._content,
                        Region = location.region._content,
                        X = double.Parse(location.longitude),
                        Y = double.Parse(location.latitude),
                        GeoTag = location.place_id,
                        Locality = location.locality._content
                    },
                DateTaken = DateTime.Parse(photo.dates.taken),
            };
        }


        private async Task<IEnumerable<BirdModel>> GetOrUpdateBirds(string title, string id,
            IClientSessionHandle clientSessionHandle, CancellationToken ct)
        {
            var inputBirdModels = ExtractBirdNames(title)
                .Where(name => name != "undefined")
                .Select(ToBirdModel)
                .ToArray();

            Program.ColoredConsole.WriteLine($"            > #{id} local: ${string.Join(", ", inputBirdModels.Select(x=> $"{x.Name}"))}", Colors.txtMuted);
            var birdsFromDb = (await GetBirdsFromDb(clientSessionHandle, inputBirdModels, ct)).ToArray();
            Program.ColoredConsole.WriteLine($"            > #{id} db: ${string.Join(", ", birdsFromDb.Select(x=> $"{x.dbModel?.Name} {x.dbModel?.Id}"))}", Colors.txtMuted);
            var toInsert = birdsFromDb
                .Where(x => x.dbModel == null)
                .Select(x => x.inputModel)
                .ToArray();

            if (!toInsert.Any())
            {
                Program.ColoredConsole.WriteLine($"            > #{id} nothing to insert for {title}", Colors.txtMuted);
                return birdsFromDb.Select(x => x.dbModel).ToList();
            }

            await _birds.InsertManyAsync(clientSessionHandle, toInsert, cancellationToken: ct);

            var updatedModels = (await GetBirdsFromDb(clientSessionHandle, inputBirdModels, ct)).ToArray();
            Program.ColoredConsole.WriteLine($"            > #{id} updated: ${string.Join(", ", updatedModels.Select(x=> $"{x.dbModel?.Name} {x.dbModel?.Id}"))}", Colors.txtMuted);

            var dbModels = updatedModels.Select(x => x.dbModel).ToList();
            if (dbModels.Any(x => x == null))
            {
                throw new Exception("some models are not saved to db");
            }

            return dbModels;
        }
        private async Task<IEnumerable<(BirdModel dbModel, BirdModel inputModel)>> GetBirdsFromDb(
            IClientSessionHandle clientSessionHandle, BirdModel[] birdModels, CancellationToken ct)
        {
            var names = birdModels.Select(x => x.Name).ToArray();
            var birdsToSearch = await _birds.FindAsync(clientSessionHandle, x => names.Contains(x.Name), cancellationToken: ct);
            var birdsDbList = await birdsToSearch.ToListAsync(ct);

            var duplicateEntries = birdsDbList.GroupBy(x => x.Name).Where(x => x.Count() > 1).ToArray(); 
            if (duplicateEntries.Any())
            {
                throw new Exception($"duplicate entry exists: {JsonSerializer.Serialize(duplicateEntries)}");
            }

            var mergedList = birdModels.Select(x => (birdsDbList.SingleOrDefault(dm => dm.Name == x.Name), x));
            return mergedList;
        }
        
        private IEnumerable<string> ExtractBirdNames(string title)
        {
            if (title.StartsWith("B: "))
                title = title["B: ".Length..];
            return title.Contains(", ") ? title.Split(", ").Distinct() : new[] {title};
        }

        private BirdModel ToBirdModel(string name)
        {
            Func<string, int> indexOf =  name.IndexOf;

            return new BirdModel
            {
                Name = name.Substring(0, indexOf("(") - 1),
                Latin = name.Substring(indexOf("(") + 1, indexOf(")") - indexOf("(") - 1)
            };
        }
    }
}