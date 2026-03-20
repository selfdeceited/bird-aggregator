using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Application.Photos;

public record GetWebsiteLinkForPhotoQuery(string BirdId) : IQuery<GetWebsiteLinkForPhotoDto>;
public record GetWebsiteLinkForPhotoDto(List<string> Links);
public class GetWebsiteLinkForPhotoQueryHandler(
    IPhotoRepository photoRepository,
    IPictureHostingService pictureHostingService)
    : IQueryHandler<GetWebsiteLinkForPhotoQuery, GetWebsiteLinkForPhotoDto>
{
    public async Task<GetWebsiteLinkForPhotoDto> Handle(GetWebsiteLinkForPhotoQuery request, CancellationToken cancellationToken)
    {
        var gallery = await photoRepository.GetGalleryForBirdAsync(request.BirdId, SortDirection.Latest);

        var links = gallery
            .Select(_ => pictureHostingService.GetAllImageLinks(_.PhotoInformation).WebsiteLink)
            .ToList();

        return new GetWebsiteLinkForPhotoDto(links);
    }
}
