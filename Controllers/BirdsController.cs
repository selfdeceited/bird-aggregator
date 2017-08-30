using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using birds.Services;
using Microsoft.Extensions.Options;
using birds.Dtos;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class BirdsController : Controller
    {
        private readonly AppSettings _settings;
        private readonly ApiContext _context;

        private readonly GalleryService _galleryService;

        public BirdsController(IOptions<AppSettings> settings, ApiContext context, GalleryService galleryService){
            _settings = settings.Value;
            _context = context;
            _galleryService = galleryService;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            return _context.Birds
                .Select(_ => new { Id = _.Id, Name = _.EnglishName, Latin = _.LatinName })
                .OrderBy(_ => _.Name)
                .ToList();
        }


        [HttpGet("gallery/{id}")]
        public IEnumerable<PhotoDto> GetGallery(int id)
        {
             var photos = _context.Photos.Where(x => x.BirdId == id).ToList()
                .OrderByDescending(x => x.DateTaken);

            return _galleryService.GetGallery(photos);
        }


        [HttpGet("lifelist")]
        public IEnumerable<object> GetLifeList()
        {
            var photos = _context.Photos.GroupBy(x => x.BirdId);

            var localList = new List<LifeListDto>();
            foreach (var item in photos)
            {
                var firstOccurence = item.Aggregate(
                    (c1, c2) => c1.DateTaken < c2.DateTaken ? c1 : c2);
                    
                if (item.Key != 0)
                    localList.Add(new LifeListDto(){ 
                        BirdId = item.Key, 
                        Name = _galleryService.GetBirdName(firstOccurence), 
                        DateMet = firstOccurence.DateTaken,
                        Location = ShowLocation(firstOccurence.LocationId),
                        LocationId = firstOccurence.LocationId
                    });
            }
            return localList.OrderByDescending(x => x.DateMet);
        }

        [HttpGet("map/markers")]
        public IEnumerable<object> GetMapMarkers()
        {
            foreach (var location in _context.Locations)
            {
                var entry = new {
                    X = location.X,
                    Y = location.Y,
                    Birds = GetBirdsByLocation(location.Id),
                    Id = location.Id
                };
                yield return entry;
            }
        }

        [HttpGet("map/markers/{id}")]
        public IEnumerable<object> GetMapMarkerByLocationId(int id)
        {
            // TODO: refactor & remove code duplication at object init
            var list = new List<object>();

            var location = _context.Locations.Find(id);
            if (location != null)
                list.Add(new {
                    X = location.X,
                    Y = location.Y,
                    Birds = GetBirdsByLocation(location.Id),
                    Id = location.Id
                });

            return list;
        }

        private IEnumerable<object> GetBirdsByLocation(int id)
        {
            var names = _context.Photos.Where(x => x.LocationId == id)
                .Select(x => new {
                    Name = _galleryService.GetBirdName(x),
                    Id = x.BirdId
                }).GroupBy(x => x.Id).Select(x => x.First());
            return names;
        }
        private string ShowLocation(int locationId)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null)
                return "unspecified location";

            Func<string, string> addComma = s => 
                string.IsNullOrEmpty(s) ? string.Empty : s + ",";

            return $"{addComma(location.Neighbourhood)} {addComma(location.Region)} {location.Country}";
        }
    }
}
