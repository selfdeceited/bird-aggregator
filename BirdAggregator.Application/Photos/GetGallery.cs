using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Application.Photos;

public record GetGalleryQueryResponse(List<PhotoDto> Photos);

public record GetGalleryQuery(int Count, SortDirection SortDirection) : IQuery<GetGalleryQueryResponse>;
public class GetGalleryQueryHandler(
    IPhotoRepository photoRepository,
    IPictureHostingService pictureHostingService)
    : IQueryHandler<GetGalleryQuery, GetGalleryQueryResponse>
{
    public async Task<GetGalleryQueryResponse> Handle(GetGalleryQuery request, CancellationToken cancellationToken)
    {
        var gallery = await photoRepository.GetAllAsync(request.Count, request.SortDirection);
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
