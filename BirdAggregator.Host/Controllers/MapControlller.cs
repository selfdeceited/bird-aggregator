using System.Threading.Tasks;
using BirdAggregator.Application.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers
{
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        private readonly IMediator _mediator;

        public MapController(IMediator mediator)
        {
	        _mediator = mediator;
        }

        [HttpGet("markers")]
        public async Task<IActionResult> GetMapMarkers()
        {
	        var markers = await _mediator.Send(new GetLocationsQuery());
            return Ok(markers);
        }

        [HttpGet("markers/{id}")]
        public async Task<IActionResult> GetMapMarkerByPhoto(string id)
        {
            var markers = await _mediator.Send(new GetLocationsByIdQuery(id));
            return Ok(markers);
        }

        [HttpGet("bird/{id}")]
        public async Task<IActionResult> GetMapMarkersByBirdId(string id)
        {
            var markers = await _mediator.Send(new GetLocationsByBirdIdQuery(id));
            return Ok(markers);
        }
        
    }
}
