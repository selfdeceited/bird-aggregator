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
            return _context.Birds
                .Select(_ => new { Id = _.Id, Name = _.EnglishName, Latin = _.LatinName })
                .OrderBy(_ => _.Name)
                .ToList();
        }


        [HttpGet("gallery/{count}")]
        public IEnumerable<PhotoDto> GetGallery(int count)
        {
             var top = _context.Photos.ToList()
                .OrderByDescending(x => x.DateTaken)
                .Take(count);

            var mapped =  top.Select(x => 
                    new PhotoDto {
                        Thumbnail = GetThumbnailUrl(x),
                        Src = GetImageUrl(x),
                        Caption = GetBirdName(x)
                    });
            return mapped;
        }

        [HttpGet("{id}/photos")]
        public IEnumerable<object> GetPhotos(int id)
        {
            return _context.Photos.Where(_ => _.BirdId == id).Select(_ => 
                $"https://www.flickr.com/photos/{_settings.FlickrUserId}/{_.FlickrId}");
        }

        [HttpGet("lifelist")]
        public IEnumerable<object> GetLifeList(){
            var photos = _context.Photos.GroupBy(x => x.BirdId);
            foreach (var item in photos)
            {
                var firstOccurence = item.Aggregate(
                    (c1, c2) => c1.DateTaken < c2.DateTaken ? c1 : c2);

                yield return new { 
                    BirdId = item.Key, 
                    Name = GetBirdName(firstOccurence), 
                    DateMet = firstOccurence.DateTaken,
                    Location = ShowLocation(firstOccurence.LocationId)
                };
            }
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
        private string ShowLocation(int locationId)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null)
                return "unspecified location";

            Func<string, string> addComma = s => 
                string.IsNullOrEmpty(s) ? string.Empty : s + ",";

            return $"{addComma(location.Neighbourhood)} {addComma(location.Region)} {addComma(location.Country)}";
        }
    }
}
