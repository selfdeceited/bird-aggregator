using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Birds.GetBirdInfo
{
    public class GetBirdInfoQueryHandler : IQueryHandler<GetBirdInfoQuery, BirdInfoDto>
    {
        private readonly IBirdRepository _birdRepository;
        private readonly IInformationService _informationService;
        public GetBirdInfoQueryHandler(IBirdRepository birdRepository, IInformationService informationService)
        {
            _birdRepository = birdRepository;
            _informationService = informationService;
        }

        public async Task<BirdInfoDto> Handle(GetBirdInfoQuery request, CancellationToken cancellationToken)
        {
            var bird = await _birdRepository.Get(request.BirdId);
            var birdInfo = await _informationService.Get(bird.EnglishName);
            return new BirdInfoDto
            {
                Name = bird.EnglishName,
                WikiInfo = birdInfo.Description,
                ImageUrl = birdInfo.ImageLink
            };
        }
    }
}