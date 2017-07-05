using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using birds.POCOs;
using birds.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class BirdsController : Controller
    {
        private readonly BirdsService _birdsService;
        private readonly AppSettings _settings;

        public BirdsController(BirdsService birdsService, IOptions<AppSettings> settings){
            _birdsService = birdsService;
            _settings = settings.Value;
        }
        
        [HttpGet]
        public IEnumerable<object> Get()
        {
            return _birdsService.GetBirds()
                .Select(_ => new { BirdName = _.Key.Replace("B: ", ""), Count = _.Value.Count() })
                .Where(_ => _.BirdName != "undefined");
        }

        [HttpGet("photos/{birdName}")]
        public IEnumerable<object> GetPhotos(string birdName)
        {
            var birdInfo = _birdsService.GetBirds()
                .Select(_ => new { BirdName = _.Key.Replace("B: ", ""), Value = _.Value })
                .Where(_ => _.BirdName == birdName)
                .Select(_ => _.Value).FirstOrDefault();
            return birdInfo?.Select(_ => $"https://www.flickr.com/photos/{_settings.FlickrUserId}/{_.id}");
        }
    }
}
