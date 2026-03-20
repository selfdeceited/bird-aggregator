using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Application.Photos;

public record GetGalleryForBirdQuery(string BirdId, SortDirection SortDirection) : IQuery<GetGalleryQueryResponse>;

public class GetGalleryForBirdQueryHandler(
    IPhotoRepository photoRepository,
    IPictureHostingService pictureHostingService)
    : IQueryHandler<GetGalleryForBirdQuery, GetGalleryQueryResponse>
{
    public async Task<GetGalleryQueryResponse> Handle(GetGalleryForBirdQuery request, CancellationToken cancellationToken)
    {
        var gallery = await photoRepository.GetGalleryForBirdAsync(request.BirdId, SortDirection.Latest);

        return new GetGalleryQueryResponse
        (
            Photos: [.. gallery.Select(_ =>
            {
                var links = pictureHostingService.GetAllImageLinks(_.PhotoInformation);
                return new PhotoDto
                (
                    Original: links.OriginalLink,
                    Src: links.ThumbnailLink,
                    Caption: _.Caption,
                    Id: _.Id,
                    DateTaken: _.DateTaken,
                    BirdIds: _.Birds.Select(x => x.Id),
                    Height: 1,
                    Width: _.Ratio,
                    Text: _.Description,
                    HostingLink: links.WebsiteLink
                );
            })]
        );
    }
}
