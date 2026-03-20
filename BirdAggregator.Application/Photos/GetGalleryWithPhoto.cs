using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Photos;

public record GetGalleryWithPhotoQuery(string PhotoId) : IQuery<GetGalleryWithPhotoDto>;
public record GetGalleryWithPhotoDto(PhotoDto Photo);

public class GetGalleryWithPhotoQueryHandler(
    IPhotoRepository photoRepository,
    IPictureHostingService pictureHostingService)
    : IQueryHandler<GetGalleryWithPhotoQuery, GetGalleryWithPhotoDto>
{
    public async Task<GetGalleryWithPhotoDto> Handle(GetGalleryWithPhotoQuery request, CancellationToken cancellationToken)
    {
        var photo = await photoRepository.GetById(request.PhotoId);
        var links = pictureHostingService.GetAllImageLinks(photo.PhotoInformation);
        var photoDto = new PhotoDto(
            Original: links.OriginalLink,
            Src: links.ThumbnailLink,
            Caption: photo.Caption,
            Id: photo.Id,
            DateTaken: photo.DateTaken,
            BirdIds: photo.Birds.Select(x => x.Id),
            Height: 1,
            Width: photo.Ratio,
            Text: photo.Description,
            HostingLink: links.WebsiteLink
        );

        return new GetGalleryWithPhotoDto(photoDto);
    }
}
