using System;
using System.Threading.Tasks;
using BirdAggregator.Application.Locations.GetLocations;
using BirdAggregator.Application.Locations.GetLocationsByBirdId;
using BirdAggregator.Application.Locations.GetLocationsById;
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
	        var orders = await _mediator.Send(new GetLocationsQuery());
            return Ok(orders);
        }

        [HttpGet("markers/{id}")]
        public async Task<IActionResult> GetMapMarkerByLocationId(int id)
        {
            var orders = await _mediator.Send(new GetLocationsByIdQuery(id));
            return Ok(orders);
        }

        [HttpGet("bird/{id}")]
        public async Task<IActionResult> GetMapMarkersByBirdId(int id)
        {
            var orders = await _mediator.Send(new GetLocationsByBirdIdQuery(id));
            return Ok(orders);
        }
        
    }
}
