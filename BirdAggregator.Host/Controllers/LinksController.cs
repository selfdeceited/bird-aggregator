using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers
{
    [Route("api/[controller]")]
    public class LinksController: Controller
    {
        [HttpGet("github")]
        public async Task<IActionResult> Github()
        {
            return Ok(string.Empty);
        }

        [HttpGet("user")]
        public async Task<IActionResult> User()
        {
            return Ok(string.Empty);
        }

        [HttpGet("flickr")]
        public async Task<IActionResult> Flickr()
        {
            return Ok(string.Empty);
        }
    }
}