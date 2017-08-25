using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using birds.POCOs;
using birds.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using birds.Dtos;
using birds.Domain;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private readonly AppSettings _settings;
        private readonly ApiContext _context;

        public PhotosController(IOptions<AppSettings> settings, ApiContext context){
            _settings = settings.Value;
            _context = context;
        }

        [HttpGet("gallery/{count}")]
        public IEnumerable<PhotoDto> GetGallery(int count)
        {
             var photos = _context.Photos.ToList()
                .OrderByDescending(x => x.DateTaken)
                .Take(count);

            foreach (var photo in photos)
            {
                yield return new PhotoDto {
                        Thumbnail = GetThumbnailUrl(photo),
                        Src = GetImageUrl(photo),
                        Caption = GetBirdName(photo),
                        Id = photo.Id
                    };
            }
        }

        [HttpGet("{id}/photos")]
        public IEnumerable<object> GetPhotos(int id)
        {
            return _context.Photos.Where(_ => _.BirdId == id).Select(_ => 
                $"https://www.flickr.com/photos/{_settings.FlickrUserId}/{_.FlickrId}");
        }

        private string GetThumbnailUrl(Domain.Photo photo)
        {
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}_n.jpg";
        }

        private string GetImageUrl(Domain.Photo photo)
        {
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}_b.jpg";
        }
        private string GetBirdName(Domain.Photo x)
        {
            var bird = _context.Birds.Find(x.BirdId);
            return bird == null ? string.Empty : bird.EnglishName;
        }
    }
}
