using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Application.Birds.GetBirdInfo;

public record BirdInfoDto(string Name, string WikiInfo, string ImageUrl);
public class GetBirdInfoQuery : IQuery<BirdInfoDto>
{
    public string BirdId { get; }
    public GetBirdInfoQuery(string id)
    {
        BirdId = id;
    }
}

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
        return new BirdInfoDto(bird.EnglishName, birdInfo.Description, birdInfo.ImageLink);
    }
}
