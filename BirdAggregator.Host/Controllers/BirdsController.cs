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

        /*
        [HttpGet("lifelist")]
        public IEnumerable<object> GetLifeList()
        {
            var allPhotos = _context.Photos.ToList();
            var grouping = _birdDao
                .GetAll()
                .ToList()
                .Select(x => new LifeListGrouping
            {
                Bird = x,
                Photos = allPhotos.Where(p => p.BirdIds.Contains(x.Id))
            }).Where(x => x.Photos.Any());

            var localList = new List<LifeListDto>();
            foreach (var item in grouping)
            {
                var firstOccurence = item.Photos.Aggregate(
                    (c1, c2) => c1.DateTaken < c2.DateTaken ? c1 : c2);
                    
                localList.Add(new LifeListDto { 
                    BirdId = item.Bird.Id, 
                    Name = item.Bird.EnglishName, 
                    DateMet = firstOccurence.DateTaken,
                    Location = ShowLocation(firstOccurence.LocationId),
                    LocationId = firstOccurence.LocationId,
                    PhotoId = firstOccurence.Id,
                });
            }
            return localList.OrderByDescending(x => x.DateMet);
        }

        [HttpGet("lifelist/peryear")]
        public IEnumerable<object> LifeListPerYear(){
            var yearsGrouping = _context.Photos.GroupBy(x => x.DateTaken.Year);
            return yearsGrouping.Select(x => new {x.Key, Count = x.SelectMany(_=>_.BirdIds).Distinct().Count()});
        }

        [HttpGet("wiki/{id}")]
        public object GetWikiInfo(int id){
            var bird = _birdDao.Find(id);
            var response = _wikipediaConnectionService.CallWikipediaExtract(bird.EnglishName);
            if (response.Contains("may refer to")){
                response = _wikipediaConnectionService.CallWikipediaExtract(bird.EnglishName + "_(bird)");
            }
            return new { Name = bird.EnglishName, WikiInfo = response, ImageUrl = string.Empty };
        }*/
    }
}
