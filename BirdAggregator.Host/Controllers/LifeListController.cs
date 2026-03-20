using BirdAggregator.Application.LifeList.GetLifeListPerYearQuery;
using BirdAggregator.Application.LifeList.GetLifeListQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers;

[Route("api/[controller]")]
public class LifeListController(IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var lifelist = await mediator.Send(new GetLifeListQuery());
        return Ok(lifelist);
    }

    [HttpGet("peryear")]
    public async Task<IActionResult> PerYear()
    {
        var lifelistPerYear = await mediator.Send(new GetLifeListPerYearQuery());
        return Ok(lifelistPerYear);
    }
}
