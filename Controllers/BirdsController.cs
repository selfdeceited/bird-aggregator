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
    public class BirdsController : Controller
    {
        private readonly AppSettings _settings;
        private readonly ApiContext _context;

        private readonly GalleryService _galleryService;
        private readonly WikipediaConnectionService _wikipediaConnectionService;

        public BirdsController(IOptions<AppSettings> settings, ApiContext context, GalleryService galleryService, WikipediaConnectionService wikipediaConnectionService)
        {
            _settings = settings.Value;
            _context = context;
            _galleryService = galleryService;
            _wikipediaConnectionService = wikipediaConnectionService;
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
                        LocationId = firstOccurence.LocationId,
                        PhotoId = firstOccurence.Id,
                    });
            }
            return localList.OrderByDescending(x => x.DateMet);
        }

        [HttpGet("wiki/{id}")]
        public object GetWikiInfo(int id){
            var bird = _context.Birds.Find(id);
            var response = _wikipediaConnectionService.CallWikipediaExtract(bird.EnglishName);
            if (response.Contains("may refer to")){
                response = _wikipediaConnectionService.CallWikipediaExtract(bird.EnglishName + "_(bird)");
            }
            return new { Name = bird.EnglishName, WikiInfo = response, ImageUrl = string.Empty };
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
