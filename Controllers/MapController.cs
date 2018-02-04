using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using birds.Services;
using Microsoft.Extensions.Options;
using birds.Dtos;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        private readonly AppSettings _settings;
        private readonly ApiContext _context;

        private readonly GalleryService _galleryService;

        public MapController(IOptions<AppSettings> settings, ApiContext context, GalleryService galleryService)
        {
            _settings = settings.Value;
            _context = context;
            _galleryService = galleryService;
        }

        [HttpGet("markers")]
        public IEnumerable<object> GetMapMarkers()
        {
            foreach (var location in _context.Locations)
            {
                var entry = new {
                    X = location.X,
                    Y = location.Y,
                    Birds = GetBirdsByLocation(location.Id),
                    Id = location.Id,
                    FirstPhotoUrl = GetPhotoByLocation(location.Id)
                };
                yield return entry;
            }
        }

        [HttpGet("markers/{id}")]
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
                    Id = location.Id,
                    FirstPhotoUrl = GetPhotoByLocation(location.Id)
                });

            return list;
        }

        [HttpGet("bird/{id}")]
        public IEnumerable<object> GetMapMarkersByBirdId(int id)
        {
            // TODO: refactor & remove code duplication at object init
            var list = new List<object>();

            var bird = _context.Birds.Find(id);
            if (bird != null)
            {
                var locationIds = _context.Photos
                    .Where(x => x.BirdId == id)
                    .Select(x => x.LocationId);

                foreach(var locationId in locationIds)
                {
                    var location = _context.Locations.Find(locationId);
                    if (location == null) continue;

                    yield return new {
                        X = location.X,
                        Y = location.Y,
                        Birds = GetBirdsByLocation(location.Id),
                        Id = location.Id,
                        FirstPhotoUrl = GetPhotoByLocation(location.Id)
                    };
                }
            }
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
        private string GetPhotoByLocation(int id)
        {
            var photo = _context.Photos.FirstOrDefault(x => x.LocationId == id);
            if (photo ==null) return string.Empty;
            var url = _galleryService.GetPreviewUrl(photo);
            return url;
        }
    }
}
