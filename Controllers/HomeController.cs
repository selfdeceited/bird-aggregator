using System;
using Microsoft.AspNetCore.Mvc;
using birds.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace birds.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _settings;
        private readonly SeedService _seedService;

        public HomeController(IOptions<AppSettings> settings, SeedService seedService){
            _seedService = seedService;
            _settings = settings.Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("reseed")] // todo: secure
        public void ReSeed()
        {
            _seedService.Seed();
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
    }
}