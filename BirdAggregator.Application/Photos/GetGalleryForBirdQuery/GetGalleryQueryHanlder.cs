using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Photos.GetGalleryForBirdQuery
{
    public class GetGalleryForBirdQueryHanlder : IQueryHandler<GetGalleryForBirdQuery, GetGalleryQueryDto>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPictureHostingService _pictureHostingService;
        public GetGalleryForBirdQueryHanlder(IPhotoRepository photoRepository, IPictureHostingService pictureHostingService)
        {
            _photoRepository = photoRepository;
            _pictureHostingService = pictureHostingService;
        }

        public async Task<GetGalleryQueryDto> Handle(GetGalleryForBirdQuery request, CancellationToken cancellationToken)
        {
            var gallery = await _photoRepository.GetGalleryForBirdAsync(request.BirdId);
            
            return new GetGalleryQueryDto {
                Photos = gallery.Select(_ => {
                    var links = _pictureHostingService.GetAllImageLinks(_.PhotoInformation);
                    return new PhotoDto {
                        Original = links.OriginalLink,
                        Src = links.ThumbnailLink,
                        Caption = _.Caption,
                        Id = _.Id,
                        DateTaken = _.DateTaken,
                        LocationId = _.Location.Id,
                        BirdIds = _.Birds.Select(x => x.Id),
                        Height = 1,
                        Width = _.Ratio,
                        Text = _.Description
                    };
                }).ToList()
            };
        }
    }
}