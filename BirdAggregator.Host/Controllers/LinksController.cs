using System.Threading.Tasks;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers
{
    [Route("api/[controller]")]
    public class LinksController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IPictureHostingService _pictureHostingService;

        public LinksController(AppSettings appSettings, IPictureHostingService pictureHostingService)
        {
            _appSettings = appSettings;
            _pictureHostingService = pictureHostingService;
        }

        [HttpGet("github")]
        public IActionResult Github()
        {
            return Ok($"https://github.com/{_appSettings.Github}");
        }

        [HttpGet("user")]
        public new IActionResult User()
        {
            return Ok(_appSettings.UserName);
        }

        [HttpGet("hosting")]
        public IActionResult Hosting()
        {
            return Ok(_pictureHostingService.GetUserLink());
        }
    }
}