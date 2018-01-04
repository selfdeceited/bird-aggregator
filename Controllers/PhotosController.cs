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

        public PhotosController(IOptions<AppSettings> settings, ApiContext context, GalleryService galleryService){
            _settings = settings.Value;
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
            return _context.Photos.Where(_ => _.BirdId == id).Select(_ => 
                $"https://www.flickr.com/photos/{_settings.FlickrUserId}/{_.FlickrId}");
        }


        [HttpGet("{id}")]
        public PhotoDto Get(int id){
            var photo = _context.Photos.Find(id);
            return _galleryService.Project(photo);
        }
    }
}
