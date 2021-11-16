using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using BirdAggregator.Infrastructure.Mongo;
using BirdAggregator.Migrator.ResponseModels;
using MongoDB.Driver;

namespace BirdAggregator.Migrator.Repositories
{
    public class PhotoWriteRepository : IPhotoWriteRepository
    {
        private readonly IMongoConnection _mongoConnection;
        private IMongoCollection<PhotoModel> _photos => _mongoConnection.Database.GetCollection<PhotoModel>("photos");
        private IMongoCollection<BirdModel> _birds => _mongoConnection.Database.GetCollection<BirdModel>("birds");

        public PhotoWriteRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }
        
        public async Task SavePhoto(SavePhotoModel savePhotoModel, CancellationToken ct)
        {
            var (photo, location, sizes) = savePhotoModel;
            var birds = await GetOrUpdateBirds(photo.title._content, ct);
            var photoModel = ToPhotoModel(photo, sizes, location, birds);
            await _photos.InsertOneAsync(photoModel, new InsertOneOptions(), ct);
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
                Ratio = largestSize != null ? (double)largestSize.width / largestSize.height : 1,
                Location = new LocationModel
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


        private async Task<IEnumerable<BirdModel>> GetOrUpdateBirds(string title, CancellationToken ct)
        {
            var inputBirdModels = ExtractBirdNames(title)
                .Where(name => name != "undefined")
                .Select(ToBirdModel)
                .ToList();

            var birdsFromDb = (await GetBirdsFromDb(inputBirdModels, ct)).ToArray();
            
            var toInsert = birdsFromDb
                .Where(x => !x.exists)
                .Select(x => x.inputModel)
                .ToArray();

            if (!toInsert.Any())
            {
                return birdsFromDb.Select(x => x.dbModel).ToList();
            }

            await _birds.InsertManyAsync(toInsert, cancellationToken: ct);


            var updatedModels = await GetBirdsFromDb(inputBirdModels, ct);

            var dbModels = updatedModels.Select(x => x.dbModel).ToList();
            if (dbModels.Any(x => x == null))
            {
                throw new Exception("some models are not saved to db");
            }

            return dbModels;
        }
        private async Task<IEnumerable<(bool exists, BirdModel dbModel, BirdModel inputModel)>> GetBirdsFromDb(IEnumerable<BirdModel> birdModels, CancellationToken ct)
        {
            return await Task.WhenAll(birdModels.Select(async model =>
            {
                var birdCursor = await _birds.FindAsync(x => x.Name == model.Name, cancellationToken: ct);
                var bird = await birdCursor.SingleOrDefaultAsync(ct);
                return bird != null ? (true, bird, model) : (false, null, model);
            }));
        }
        
        private IEnumerable<string> ExtractBirdNames(string title)
        {
            if (title.StartsWith("B: "))
                title = title["B: ".Length..];
            return title.Contains(", ") ? title.Split(", ") : new[] {title};
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