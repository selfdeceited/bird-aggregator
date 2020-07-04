using BirdAggregator.Application.LifeList.GetLifeListPerYearQuery;
using BirdAggregator.Application.LifeList.GetLifeListQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace birds.Controllers
{
    [Route("api/[controller]")]
    public class LifeListController: Controller
    {
        private readonly IMediator _mediator;

        public LifeListController(IMediator mediator)
        {
	        _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var lifelist = await _mediator.Send(new GetLifeListQuery());
            return Ok(lifelist);
        }

        [HttpGet("peryear")]
        public async Task<IActionResult> PerYear()
        {
            var lifelistPerYear = await _mediator.Send(new GetLifeListPerYearQuery());
            return Ok(lifelistPerYear);
        }
    }
}