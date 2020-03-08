using Microsoft.AspNetCore.Mvc;
using birds.Services;
using Microsoft.Extensions.Options;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class MetaController : Controller
    {
        private readonly AppSettings _settings;

        public MetaController(IOptions<AppSettings> settings, SeedService seedService){
            _settings = settings.Value;
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
    }
}
