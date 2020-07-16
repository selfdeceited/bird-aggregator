using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;

namespace BirdAggregator.Application.Birds.GetBirds
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