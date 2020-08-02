using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.Flickr;
using Newtonsoft.Json;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class PhotoRepository: IPhotoRepository
    {
        private readonly IBirdRepository _birdRepository;
        private readonly ILocationRepository _locationRepository;

        public PhotoRepository(IBirdRepository birdRepository, ILocationRepository locationRepository)
        {
            _birdRepository = birdRepository;
            _locationRepository = locationRepository;
        }

        public async Task<List<Photo>> GetAllAsync()
        {
            var fileContent = await File.ReadAllTextAsync(@"../data/data.photos.json");
            
            var photoModels = JsonConvert.DeserializeObject<List<PhotoModel>>(fileContent);
            var birdResults = await Task.WhenAll(photoModels.Select(model => _birdRepository.GetBirdsByIds(model.BirdIds)));
            var locationResults = await Task.WhenAll(photoModels.Select(model => _locationRepository.GetByIdAsync(model.LocationId)));

            Photo Project((List<Bird> birds, PhotoModel model, Location location) tuple)
            {
                var (birds, model, location) = tuple;
                var photoInformation = new FlickrPhotoInformation(model.FlickrId, model.FarmId, model.ServerId, model.Secret);
                return new Photo(model.Id, location, photoInformation, birds, model.DateTaken, model.Ratio, model.Description);
            }
            
            return birdResults.ToList()
                .Zip(photoModels,  (birds, photo) => (birds, photo))
                .Zip(locationResults,  (_, location) => (_.birds, _.photo, location))
                .Select(Project).ToList();
        }

        public async Task<List<Photo>> GetAllAsync(int count)
        {
            var allPhotos = await GetAllAsync();
            return allPhotos.Take(count).ToList();
        }

        public async Task<Photo> GetById(int photoId)
        {
            var allPhotos = await GetAllAsync();
            return allPhotos.SingleOrDefault(x => x.Id == photoId);
        }

        public async Task<List<Photo>> GetGalleryForBirdAsync(int birdId)
        {
            var allPhotos = await GetAllAsync();

            return allPhotos.Where(_ => _.Birds
                    .Select(bird => bird.Id)
                    .Contains(birdId))
                .OrderByDescending(_ => _.DateTaken).ToList();
        }
    }
}