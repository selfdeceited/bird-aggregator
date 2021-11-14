using System.Threading.Tasks;
using BirdAggregator.Application.Photos.GetGalleryForBirdQuery;
using BirdAggregator.Application.Photos.GetGalleryQuery;
using BirdAggregator.Application.Photos.GetGalleryWithPhotoQuery;
using BirdAggregator.Application.Photos.GetWebsiteLinkForPhotoQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers
{
    [Route("api/[controller]")]
    public class GalleryController : Controller
    {
        private readonly IMediator _mediator;

        public GalleryController(IMediator mediator)
        {
	        _mediator = mediator;
        }

        [HttpGet("bird/{id}")]
        public async Task<IActionResult> GetForBird(string id)
        {
            var gallery = await _mediator.Send(new GetGalleryForBirdQuery(id));
            return Ok(gallery);
        }
        
        [HttpGet("{count}")]
        public async Task<IActionResult> GetGallery(int count)
        {
            var gallery = await _mediator.Send(new GetGalleryQuery(count));
            return Ok(gallery);
        }

        [HttpGet("photo/{id}")]
        public async Task<IActionResult> GetForPhoto(string id)
        {
            var gallery = await _mediator.Send(new GetGalleryWithPhotoQuery(id));
            return Ok(gallery);
        }

        [HttpGet("photo/{id}/websitelink")]
        public async Task<IActionResult> GetWebsiteLinkForPhoto(string id)
        {
            var gallery = await _mediator.Send(new GetWebsiteLinkForPhotoQuery(id));
            return Ok(gallery);
        }
    }
}