using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BirdAggregator.Application.Photos.GetGalleryForBirdQuery;
using BirdAggregator.Application.Photos.GetGalleryQuery;

namespace birds.Controllers
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
        public async Task<IActionResult> GetForBird(int id)
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
    }
}