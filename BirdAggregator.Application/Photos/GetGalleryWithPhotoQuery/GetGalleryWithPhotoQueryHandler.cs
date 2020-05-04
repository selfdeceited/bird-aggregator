using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Photos.GetGalleryWithPhotoQuery
{
    public class GetGalleryWithPhotoQueryHanlder: IQueryHandler<GetGalleryWithPhotoQuery, GetGalleryWithPhotoDto>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPictureHostingService _pictureHostingService;
        
        public GetGalleryWithPhotoQueryHanlder(IPhotoRepository photoRepository, IPictureHostingService pictureHostingService)
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
                        LocationId = photo.Location.Id,
                        BirdIds = photo.Birds.Select(x => x.Id),
                        Height = 1,
                        Width = photo.Ratio,
                        Text = photo.Description
            };
            
            return new GetGalleryWithPhotoDto{ Photo = photoDto };
        }
    }
}