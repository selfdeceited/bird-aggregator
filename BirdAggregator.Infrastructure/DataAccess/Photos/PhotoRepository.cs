using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.DataAccess.Mappings;
using BirdAggregator.Infrastructure.Mongo;
using MongoDB.Driver;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class PhotoRepository : IPhotoRepository, IBirdRepository, ILocationRepository
    {
        private readonly IMongoConnection _mongoConnection;
        private IMongoCollection<PhotoModel> _photos => _mongoConnection.Database.GetCollection<PhotoModel>("photos");
        private IMongoCollection<BirdModel> _birds => _mongoConnection.Database.GetCollection<BirdModel>("birds");

        private readonly BirdMapper _birdMapper = new();
        private readonly PhotoMapper _photoMapper = new();
        private readonly LocationMapper _locationMapper = new();

        public PhotoRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }

        public async Task<Bird> Get(string birdId)
        {
            var birdCursor = await _birds.FindAsync(_ => _.Id.ToString() == birdId);
            var birdModel = await birdCursor.SingleAsync();
            return _birdMapper.ToDomain(birdModel);
        }

        public async Task<Bird[]> GetAll()
        {
            var birdCursor = await _birds.FindAsync<BirdModel>(_ => true);
            var birdModels = await birdCursor.ToListAsync();
            return birdModels.Select(_birdMapper.ToDomain).ToArray();
        }
        public async Task<Bird[]> GetBirdsByIds(IEnumerable<string> birdIds)
        {
            var birdCursor = await _birds.FindAsync(_ => birdIds.Contains(_.Id.ToString()));
            var birdModels = await birdCursor.ToListAsync();
            return birdModels.Select(_birdMapper.ToDomain).ToArray();
        }

        public async Task<Location[]> GetAllAsync()
        {
            var locations = await GetPhotoResultModelLookup()
                .Project(x => new { x.Location })
                .ToListAsync();

            return locations
                .Select(_ => _locationMapper.ToDomain(_.Location))
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


        async Task<Photo> IPhotoRepository.GetById(string photoId)
        {
            var photoAggregate = await GetPhotoResultModelLookup()
                .Match(x => x.Id.ToString() == photoId)
                .ToListAsync();


            var photoResultModel = photoAggregate.Single();
            return _photoMapper.ToDomain(photoResultModel);
        }

        async Task<Photo[]> IPhotoRepository.GetGalleryForBirdAsync(string birdId)
        {
            var photoAggregate = await GetPhotoResultModelLookup()
                .Match(x => x.BirdIds.Select(o => o.ToString()).Contains(birdId))
                .ToListAsync();

            return photoAggregate
                .Select(_photoMapper.ToDomain)
                .ToArray();
        }

        private IAggregateFluent<PhotoResultModel> GetPhotoResultModelLookup()
        {
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

        public async Task<Photo> GetByHostingId(string hostingId)
        {
           var model = await GetPhotoResultModelLookup()
                .Match(x => x.Flickr.Id == hostingId)
                .FirstOrDefaultAsync();
           return model == null ? null : _photoMapper.ToDomain(model);
        }
    }
}