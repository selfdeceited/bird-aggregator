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

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class BirdsController : Controller
    {
        private readonly AppSettings _settings;
        private readonly ApiContext _context;

        public BirdsController(IOptions<AppSettings> settings, ApiContext context){
            _settings = settings.Value;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            return _context.Birds.Select(_ => new { Id = _.Id, Name = _.EnglishName, Latin = _.LatinName }).ToList();
        }


        [HttpGet("gallery")]
        public IEnumerable<PhotoDto> GetGallery()
        {
            return _context.Photos.ToList().Select(x => 
                new PhotoDto {
                    Thumbnail = GetThumbnailUrl(x),
                    Src = GetImageUrl(x),
                    Caption = GetBirdName(x)
                });
        }

        [HttpGet("{id}/photos")]
        public IEnumerable<object> GetPhotos(int id)
        {
            return _context.Photos.Where(_ => _.BirdId == id).Select(_ => 
                $"https://www.flickr.com/photos/{_settings.FlickrUserId}/{_.FlickrId}");
        }

        [HttpGet("{id}/locations")]
        public IEnumerable<object> GetLocations(int id)
        {
            var photos = _context.Birds.SingleOrDefault(x => x.Id == id)?.Photos.Select(x => x.Id);
            return _context.Locations.Where(_ => photos.Contains(_.PhotoId));
        }

        private string GetThumbnailUrl(Domain.Photo photo){
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}_n.jpg";
        }

        private string GetImageUrl(Domain.Photo photo){
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}_b.jpg";
        }

        private string GetBirdName(Domain.Photo x){
            var bird = _context.Birds.Find(x.BirdId);
            return bird == null ? string.Empty : bird.EnglishName;
        }
    }
}
