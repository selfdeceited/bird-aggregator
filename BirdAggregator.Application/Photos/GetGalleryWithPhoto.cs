using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Photos
{
    public record GetGalleryWithPhotoQuery (string PhotoId) : IQuery<GetGalleryWithPhotoDto>;
    public record GetGalleryWithPhotoDto (PhotoDto Photo);
    public class GetGalleryWithPhotoQueryHandler: IQueryHandler<GetGalleryWithPhotoQuery, GetGalleryWithPhotoDto>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPictureHostingService _pictureHostingService;
        
        public GetGalleryWithPhotoQueryHandler(IPhotoRepository photoRepository, IPictureHostingService pictureHostingService)
        {
            _photoRepository = photoRepository;
            _pictureHostingService = pictureHostingService;
        }

        public async Task<GetGalleryWithPhotoDto> Handle(GetGalleryWithPhotoQuery request, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.GetById(request.PhotoId);
            var links = _pictureHostingService.GetAllImageLinks(photo.PhotoInformation);
            var photoDto = new PhotoDto {
                        Original = links.OriginalLink,
                        Src = links.ThumbnailLink,
                        Caption = photo.Caption,
                        Id = photo.Id,
                        DateTaken = photo.DateTaken,
                        BirdIds = photo.Birds.Select(x => x.Id),
                        Height = 1,
                        Width = photo.Ratio,
                        Text = photo.Description,
                        HostingLink = links.WebsiteLink,
            };
            
            return new GetGalleryWithPhotoDto(photoDto);
        }
    }
}