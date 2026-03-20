using BirdAggregator.Application.Birds;
using BirdAggregator.Application.Birds.GetBirds;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers;

[Route("api/[controller]")]
public class BirdsController(IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var orders = await mediator.Send(new GetBirdsQuery());
        return Ok(orders);
    }

    [HttpGet("info/{id}")]
    public async Task<IActionResult> GetInfo(string id)
    {
        var info = await mediator.Send(new GetBirdInfoQuery(id));
        return Ok(info);
    }
}
