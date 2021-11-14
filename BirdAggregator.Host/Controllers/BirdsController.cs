using System.Threading.Tasks;
using BirdAggregator.Application.Birds.GetBirdInfo;
using BirdAggregator.Application.Birds.GetBirds;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BirdAggregator.Host.Controllers
{
    [Route("api/[controller]")]
    public class BirdsController : Controller
    {
        private readonly IMediator _mediator;

        public BirdsController(IMediator mediator)
        {
	        _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _mediator.Send(new GetBirdsQuery());
            return Ok(orders);
        }

        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetInfo(string id)
        {
            var info = await _mediator.Send(new GetBirdInfoQuery(id));
            return Ok(info);
        }
    }
}
