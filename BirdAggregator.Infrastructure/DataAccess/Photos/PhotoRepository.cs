using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.DataAccess.Mappings;
using BirdAggregator.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class PhotoRepository : IPhotoRepository, IBirdRepository, ILocationRepository
    {
        private readonly IMongoConnection _mongoConnection;
        private IMongoCollection<PhotoModel> _photos => _mongoConnection.Database.GetCollection<PhotoModel>("photos");
        private IMongoCollection<BirdModel> _birds => _mongoConnection.Database.GetCollection<BirdModel>("birds");
        private IMongoCollection<LocationModel> _locations => _mongoConnection.Database.GetCollection<LocationModel>("locations");

        private readonly BirdMapper _birdMapper = new BirdMapper();
        private readonly PhotoMapper _photoMapper = new PhotoMapper();
         private readonly LocationMapper _locationMapper = new LocationMapper();

        public PhotoRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }

        public async Task<Bird> Get(int birdId)
        {
            var birdCursor = await _birds.FindAsync<BirdModel>(_ => _.Id == birdId);
            var birdModel = await birdCursor.SingleAsync();
            return _birdMapper.ToDomain(birdModel);
        }

        public async Task<Bird[]> GetAll()
        {
            var birdCursor = await _birds.FindAsync<BirdModel>(_ => true);
            var birdModels = await birdCursor.ToListAsync();
            return birdModels.Select(_birdMapper.ToDomain).ToArray();
        }
        public async Task<Bird[]> GetBirdsByIds(IEnumerable<int> birdIds)
        {
            var birdCursor = await _birds.FindAsync<BirdModel>(_ => birdIds.Contains(_.Id));
            var birdModels = await birdCursor.ToListAsync();
            return birdModels.Select(_birdMapper.ToDomain).ToArray();
        }

        public async Task<Location[]> GetAllAsync()
        {
            // todo: doesn't work. System.FormatException: Element 'Location' does not match any field or property of class 
            var projection = Builders<PhotoResultModel>.Projection.Include(x => x.Location);
            var locations = await GetPhotoResultModelLookup()
                .Project<LocationModel>(projection)
                .ToListAsync();

            return locations
                .Select(_locationMapper.ToDomain)
                .ToArray();
        }

        async Task<Photo[]> IPhotoRepository.GetAllAsync()
        {
            var photoAggregate = await GetPhotoResultModelLookup()
                .ToListAsync();

            return photoAggregate
                .Select(_photoMapper.ToDomain)
                .ToArray();
        }

        async Task<Photo[]> IPhotoRepository.GetAllAsync(int count)
        {
            var photoAggregate = await GetPhotoResultModelLookup()
                .Limit(count)
                .ToListAsync();

            return photoAggregate
                .Select(_photoMapper.ToDomain)
                .ToArray();
        }

        async Task<Location[]> IPhotoRepository.GetByBirdIdAsync(int birdId)
        {
            // todo: doesn't work. System.FormatException: Element 'Location' does not match any field or property of class 
            var projection = Builders<PhotoResultModel>.Projection.Include(x => x.Location);
            var locations = await GetPhotoResultModelLookup()
                .Match<PhotoResultModel>(x => x.BirdIds.Contains(birdId))
                .Project<LocationModel>(projection)
                .ToListAsync();

            return locations
                .Select(_locationMapper.ToDomain)
                .ToArray();
        }

        async Task<Photo> IPhotoRepository.GetById(int photoId)
        {
            var photoAggregate = await GetPhotoResultModelLookup()
                .Match<PhotoResultModel>(x => x._id == photoId)
                .ToListAsync();
            

            var photoResultModel = photoAggregate.Single();
            return _photoMapper.ToDomain(photoResultModel);
        }

        async Task<Photo[]> IPhotoRepository.GetGalleryForBirdAsync(int birdId)
        {
            var photoAggregate = await GetPhotoResultModelLookup()
                .Match<PhotoResultModel>(x => x.BirdIds.Contains(birdId))
                .ToListAsync();

            return photoAggregate
                .Select(_photoMapper.ToDomain)
                .ToArray();
        }

        private IMongoQueryable<T> Query<T>(string name) =>
            _mongoConnection.Database.GetCollection<T>(name).AsQueryable();


        private IAggregateFluent<PhotoResultModel> GetPhotoResultModelLookup() {
            return _photos
                .Aggregate()
                .Lookup<PhotoModel, BirdModel, PhotoResultModel>(
                    _birds,
                    x => x.BirdIds,
                    x => x.Id,
                    x => x.BirdModels);
        }

        // todo: consider switching to /map/photo/{id}
        public Task<Location> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        Task<Photo[]> IPhotoRepository.GetByLocationAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}