using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Application.Photos
{
    public record GetGalleryForBirdQuery (string BirdId, SortDirection SortDirection) : IQuery<GetGalleryQueryResponse>;

    public class GetGalleryForBirdQueryHandler : IQueryHandler<GetGalleryForBirdQuery, GetGalleryQueryResponse>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPictureHostingService _pictureHostingService;
        public GetGalleryForBirdQueryHandler(IPhotoRepository photoRepository, IPictureHostingService pictureHostingService)
        {
            _photoRepository = photoRepository;
            _pictureHostingService = pictureHostingService;
        }

        public async Task<GetGalleryQueryResponse> Handle(GetGalleryForBirdQuery request, CancellationToken cancellationToken)
        {
            var gallery = await _photoRepository.GetGalleryForBirdAsync(request.BirdId);
            
            return new GetGalleryQueryResponse {
                Photos = gallery.Select(_ => {
                    var links = _pictureHostingService.GetAllImageLinks(_.PhotoInformation);
                    return new PhotoDto {
                        Original = links.OriginalLink,
                        Src = links.ThumbnailLink,
                        Caption = _.Caption,
                        Id = _.Id,
                        DateTaken = _.DateTaken,
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