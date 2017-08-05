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
    public class MetaController : Controller
    {
        private readonly AppSettings _settings;
        private readonly SeedService _seedService;

        public MetaController(IOptions<AppSettings> settings, SeedService seedService){
            _settings = settings.Value;
            _seedService = seedService;
        }

        [HttpGet("github")]
        public string GetGithubLink()
        {
            return $"https://github.com/{_settings.Github}";
        }

        [HttpGet("flickr")]
        public string GetFlickrLink()
        {
            return $"https://www.flickr.com/photos/{_settings.FlickrUserId}/";
        }

        [HttpGet("user")]
        public string GetUser(){
            return _settings.UserName;
        }

        [HttpPost("reseed")] // todo: secure
        public void ReSeed()
        {
            _seedService.Seed();
        }
    }
}
