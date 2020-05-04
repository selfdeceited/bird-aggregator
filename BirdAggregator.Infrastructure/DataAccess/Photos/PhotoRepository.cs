using BirdAggregator.Domain.Photos;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Infrastructure.Flickr;

namespace BirdAggregator.Infrastructure.DataAccess.Birds
{
    public class PhotoRepository: IPhotoRepository {
        private readonly IBirdRepository _birdRepository;

        public PhotoRepository(IBirdRepository birdRepository)
        {
            _birdRepository = birdRepository;
        }

        public async Task<List<Photo>> GetAllAsync() {
            var fileContent = await File.ReadAllTextAsync(@"../data/data.photos.json");
            
            var photoModels = JsonConvert.DeserializeObject<List<PhotoModel>>(fileContent);
            var birdResults = await Task.WhenAll(photoModels.Select(model => _birdRepository.GetBirdsByIds(model.BirdIds)));

            Photo Project((List<Bird> birds, PhotoModel model) tuple)
            {
                var model = tuple.model;
                var photoInformation = new FlickrPhotoInformation(model.FlickrId, model.FarmId, model.ServerId, model.Secret);
                return new Photo(model.Id, model.LocationId, photoInformation, tuple.birds, model.DateTaken, model.Ratio, model.Description);
            }
            
            return birdResults.ToList()
                .Zip(photoModels, (a, b) => (a, b))
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