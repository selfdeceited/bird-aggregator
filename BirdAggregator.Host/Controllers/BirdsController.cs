using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using BirdAggregator.Application.Birds.GetBirdsQuery;

namespace birds.Controllers
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
    }
}
