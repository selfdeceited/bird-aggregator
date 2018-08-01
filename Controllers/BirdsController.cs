using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using birds.Services;
using birds.Dtos;
using birds.Dao;
using Microsoft.Extensions.Options;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class BirdsController : Controller
    {
	    private readonly AppSettings _settings;
		private readonly ApiContext _context;
        private readonly GalleryService _galleryService;
        private readonly WikipediaConnectionService _wikipediaConnectionService;
        private readonly BirdDao _birdDao;

        public BirdsController(IOptions<AppSettings> settings, ApiContext context, GalleryService galleryService, WikipediaConnectionService wikipediaConnectionService, BirdDao birdDao)
        {
	        _settings = settings.Value;
			_context = context;
            _galleryService = galleryService;
            _wikipediaConnectionService = wikipediaConnectionService;
            _birdDao = birdDao;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            return _birdDao.GetAll()
                .Select(_ => new { Id = _.Id, Name = _.EnglishName, Latin = _.LatinName })
                .OrderBy(_ => _.Name)
                .ToList();
        }


        [HttpGet("gallery/{id}")]
        public IEnumerable<PhotoDto> GetGallery(int id)
        {
             var photos = _context.Photos.Where(x => x.BirdIds.Contains(id)).ToList()
                .OrderByDescending(x => x.DateTaken);

            return _galleryService.GetGallery(photos);
        }

        [HttpGet("lifelist")]
        public IEnumerable<object> GetLifeList()
        {
            var grouping = _birdDao.GetAll().Select(x => new LifeListGrouping
            {
                Bird = x,
                Photos = _context.Photos.Where(p => p.BirdIds.Contains(x.Id))
            }).Where(x => x.Photos.Any());

            var localList = new List<LifeListDto>();
            foreach (var item in grouping)
            {
                var firstOccurence = item.Photos.Aggregate(
                    (c1, c2) => c1.DateTaken < c2.DateTaken ? c1 : c2);
                    
                localList.Add(new LifeListDto { 
                    BirdId = item.Bird.Id, 
                    Name = item.Bird.EnglishName, 
                    DateMet = firstOccurence.DateTaken,
                    Location = ShowLocation(firstOccurence.LocationId),
                    LocationId = firstOccurence.LocationId,
                    PhotoId = firstOccurence.Id,
                });
            }
            return localList.OrderByDescending(x => x.DateMet);
        }

        [HttpGet("lifelist/peryear")]
        public IEnumerable<object> LifeListPerYear(){
            var yearsGrouping = _context.Photos.GroupBy(x => x.DateTaken.Year);
            return yearsGrouping.Select(x => new {x.Key, Count = x.SelectMany(_=>_.BirdIds).Distinct().Count()});
        }

        [HttpGet("wiki/{id}")]
        public object GetWikiInfo(int id){
            var bird = _birdDao.Find(id);
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

        private class LifeListGrouping
        {
            public Domain.Bird Bird {get;set;}
            public IEnumerable<Domain.Photo> Photos {get; set;}
        }
    }
}
