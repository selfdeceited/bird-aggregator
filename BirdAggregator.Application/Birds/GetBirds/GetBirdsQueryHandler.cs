using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application;
using BirdAggregator.Application.Birds.GetBirdsQuery;
using System.Linq;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Application.Birds;
 
namespace SampleProject.Application.Customers.GetCustomerDetails
{
    public class GetBirdsQueryHandler : IQueryHandler<GetBirdsQuery, BirdListDto>
    {
        private readonly IBirdRepository _birdRepository;
        public GetBirdsQueryHandler(IBirdRepository birdRepository)
        {
            _birdRepository = birdRepository;
        }

        public async Task<BirdListDto> Handle(GetBirdsQuery request, CancellationToken cancellationToken)
        {
            var allBirds = await _birdRepository.GetAllAsync();          

            return new BirdListDto {
                Birds = allBirds.Select(bird => new BirdDto {
                    Id = bird.Id,
                    Latin = bird.LatinName,
                    Name = bird.EnglishName
                }).ToList()
                // TODO: add Automapper!
            };
        }
    }
}