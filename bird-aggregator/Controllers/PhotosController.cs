using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using birds.Services;
using Microsoft.Extensions.Options;
using birds.Dtos;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private readonly AppSettings _settings;
        private readonly ApiContext _context;
        private readonly GalleryService _galleryService;

        public PhotosController(IOptionsMonitor<AppSettings> settings, ApiContext context, GalleryService galleryService){
            _settings = settings.CurrentValue;
            _context = context;
            _galleryService = galleryService;
        }

        [HttpGet("gallery/{count}")]
        public IEnumerable<PhotoDto> GetGallery(int count)
        {
            var photos = _context.Photos.ToList()
                .OrderByDescending(x => x.DateTaken)
                .Take(count);

            return _galleryService.GetGallery(photos);
        }

        [HttpGet("{id}/photos")]
        public IEnumerable<object> GetPhotos(int id)
        {
            return _context.Photos.Where(_ => _.BirdIds.Contains(id)).Select(_ => 
                $"https://www.flickr.com/photos/{_settings.FlickrUserId}/{_.FlickrId}");
        }


        [HttpGet("{id}")]
        public PhotoDto Get(int id){
            var photo = _context.Photos.Find(id);
            return _galleryService.Project(photo);
        }

        // Add for flat schema migration.
        [HttpGet("flat")]
        public IEnumerable<object> Flat() {
            var allBirds = _context.Birds.ToList();
            var allLocations = _context.Locations.ToList();

            return _context.Photos.ToList().Select(_ => {
                var location = allLocations.Single(x => x.Id == _.LocationId);
                return new {
                Id = _.Id,
                BirdIds = _.BirdIds,
                Flickr = new {
                    Id = _.FlickrId,
                    FarmId = _.FarmId,
                    ServerId = _.ServerId,
                    Secret = _.Secret,
                },

                Location = new {
                    Id = location.Id,
                    Neighborhood = location.Neighbourhood,
                    Region = location.Region,
                    GeoTag = location.GeoTag,
                    Country = location.Country,
                    X = location.X,
                    Y = location.Y,
                },
                DateTaken = _.DateTaken,
                Description = _.Description,
                Ratio = _.Ratio
            };
            }
            );
              
        }
    }
}
