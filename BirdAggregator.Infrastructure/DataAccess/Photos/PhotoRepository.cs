using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.DataAccess.Birds;
using BirdAggregator.Infrastructure.DataAccess.Locations;
using BirdAggregator.Infrastructure.Flickr;
using BirdAggregator.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public partial class PhotoRepository : IPhotoRepository
    {
        private readonly IMongoConnection _mongoConnection;
        private IMongoCollection<PhotoModel> _photos => _mongoConnection.Database.GetCollection<PhotoModel>("photos");

        public PhotoRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }

        public async Task<List<Photo>> GetAllAsync()
        {
            var list = FetchAll(9000).ToList();
            return list.Select(MapModel).ToList();
        }

        public async Task<List<Photo>> GetAllAsync(int count)
        {
            var list = FetchAll(count).ToList();
            return list.Select(MapModel).ToList();
        }

        private IEnumerable<PhotoResultModel> FetchAll(int count)
        {
            // todo: completely rewrite. No lazy joins / lookup aggregates work by now
            var locations = Query<LocationModel>("locations").ToList();
            var birds = Query<BirdModel>("birds").ToList();
            var photos = Query<PhotoModel>("photos").Take(count).ToList();


            var result = photos.Join(locations,
                                p => p.LocationId,
                                l => l.Id,
                                (p, l) => new PhotoResultModel(p, birds.Where(b => p.BirdIds.Contains(b.Id)), l)).ToList();

            return result;
        }


        public async Task<Photo> GetById(int photoId)
        {
            var photoFind = await _photos.FindAsync(x => x.Id == photoId);
            var photoModel = await photoFind.SingleOrDefaultAsync();
            var birds = await Query<BirdModel>("birds").Where(x => photoModel.BirdIds.Contains(x.Id)).ToListAsync();
            var locationModel = await Query<LocationModel>("locations").SingleOrDefaultAsync(l => l.Id == photoModel.Id);
            return MapModel(new PhotoResultModel(photoModel, birds.ToArray(), locationModel));
        }

        public async Task<List<Photo>> GetByLocationAsync(int id)
        {
            var list = FetchAll(9000)
                .Where(x => x.PhotoModel.LocationId == id).ToList();

            return list.Select(MapModel).ToList();
        }

        public async Task<List<Photo>> GetGalleryForBirdAsync(int birdId)
        {
            var list = FetchAll(9000)
                .Where(x => x.BirdModels.Select(b => b.Id).Contains(birdId))
                .ToList();

            return list.Select(MapModel).ToList();
        }

        public async Task<IEnumerable<Location>> GetByBirdIdAsync(int birdId)
        {
            var photos = await GetGalleryForBirdAsync(birdId);
            return photos.Select(x => x.Location);
        }

        private Photo MapModel(PhotoResultModel photoResultModel)
        {
            var model = photoResultModel.PhotoModel;
            var locationModel = photoResultModel.LocationModel;
            var birdModels = photoResultModel.BirdModels;

            var location = LocationRepository.MapModel(locationModel);
            var birds = birdModels.Select(BirdRepository.MapModel);
            var photoInformation = new FlickrPhotoInformation(model.FlickrId, model.FarmId, model.ServerId, model.Secret);
            return new Photo(model.Id, location, photoInformation, birds, model.DateTaken, model.Ratio, model.Description);
        }

        private IMongoQueryable<T> Query<T>(string name) =>
                _mongoConnection.Database.GetCollection<T>(name).AsQueryable();
    }
}