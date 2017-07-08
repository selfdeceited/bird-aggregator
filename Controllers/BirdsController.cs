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
        private readonly AppSettings _settings;
        private readonly ApiContext _context;

        public BirdsController(IOptions<AppSettings> settings, ApiContext context){
            _settings = settings.Value;
            _context = context;
        }

        [HttpGet]
        public IEnumerable<object> Get()
        {
            return _context.Birds.Select(_ => new { Id = _.Id, Name = _.EnglishName }).ToList();
        }

        [HttpGet("{id}/photos")]
        public IEnumerable<object> GetPhotos(int id)
        {
            var bird = _context.Birds.SingleOrDefault(x => x.Id == id);
            return bird?.Photos?.Select(_ => 
                $"https://www.flickr.com/photos/{_settings.FlickrUserId}/{_.FlickrId}");
        }
    }
}
