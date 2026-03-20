using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Application.Birds;

public record BirdInfoDto(string Name, string WikiInfo, string ImageUrl);
public class GetBirdInfoQuery(string id) : IQuery<BirdInfoDto>
{
    public string BirdId { get; } = id;
}

public class GetBirdInfoQueryHandler(
    IBirdRepository birdRepository,
    IInformationService informationService)
    : IQueryHandler<GetBirdInfoQuery, BirdInfoDto>
{
    public async Task<BirdInfoDto> Handle(GetBirdInfoQuery request, CancellationToken cancellationToken)
    {
        var bird = await birdRepository.Get(request.BirdId);
        var birdInfo = await informationService.Get(bird.EnglishName);
        return new BirdInfoDto(bird.EnglishName, birdInfo.Description, birdInfo.ImageLink);
    }
}
