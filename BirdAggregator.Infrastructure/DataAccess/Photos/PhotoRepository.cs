using System.Collections.Generic;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class PhotoRepository : IPhotoRepository, IBirdRepository, ILocationRepository
    {
        private readonly IMongoConnection _mongoConnection;
        private IMongoCollection<PhotoModel> _photos => _mongoConnection.Database.GetCollection<PhotoModel>("photos");


        public Task<Bird> Get(int birdId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Bird>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Location>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Bird>> GetBirdsByIds(IEnumerable<int> birdIds)
        {
            throw new System.NotImplementedException();
        }

        public Task<Location> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }
        Task<List<Photo>> IPhotoRepository.GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        Task<List<Photo>> IPhotoRepository.GetAllAsync(int count)
        {
            throw new System.NotImplementedException();
        }

        Task<IEnumerable<Location>> IPhotoRepository.GetByBirdIdAsync(int birdId)
        {
            throw new System.NotImplementedException();
        }

        async Task<Photo> IPhotoRepository.GetById(int photoId)
        {
            var photoFind = await _photos.FindAsync(x => x.Id == photoId);
            var photoModel = await photoFind.SingleOrDefaultAsync();

            return MapModel(photoModel);
        }

        Task<List<Photo>> IPhotoRepository.GetByLocationAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        Task<List<Photo>> IPhotoRepository.GetGalleryForBirdAsync(int birdId)
        {
            throw new System.NotImplementedException();
        }

        private Photo MapModel(PhotoModel photoModel)
        {
            throw new System.NotImplementedException();
        }

        private IMongoQueryable<T> Query<T>(string name) =>
        _mongoConnection.Database.GetCollection<T>(name).AsQueryable();
    }
}