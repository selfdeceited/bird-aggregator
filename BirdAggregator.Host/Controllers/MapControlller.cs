using BirdAggregator.Application.Locations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers;

[Route("api/[controller]")]
public class MapController(IMediator mediator) : Controller
{
    [HttpGet("markers")]
    public async Task<IActionResult> GetMapMarkers()
    {
        var markers = await mediator.Send(new GetLocationsQuery());
        return Ok(markers);
    }

    [HttpGet("markers/{id}")]
    public async Task<IActionResult> GetMapMarkerByPhoto(string id)
    {
        var markers = await mediator.Send(new GetLocationsByIdQuery(id));
        return Ok(markers);
    }

    [HttpGet("bird/{id}")]
    public async Task<IActionResult> GetMapMarkersByBirdId(string id)
    {
        var markers = await mediator.Send(new GetLocationsByBirdIdQuery(id));
        return Ok(markers);
    }
}
