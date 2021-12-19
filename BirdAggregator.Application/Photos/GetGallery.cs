using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Application.Photos
{
    public record GetGalleryQuery(int Count, SortDirection SortDirection) : IQuery<GetGalleryQueryResponse>;
    public class GetGalleryQueryHandler: IQueryHandler<GetGalleryQuery, GetGalleryQueryResponse>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPictureHostingService _pictureHostingService;
        public GetGalleryQueryHandler(IPhotoRepository photoRepository, IPictureHostingService pictureHostingService)
        {
            _photoRepository = photoRepository;
            _pictureHostingService = pictureHostingService;
        }

        public async Task<GetGalleryQueryResponse> Handle(GetGalleryQuery request, CancellationToken cancellationToken)
        {
            var gallery = await _photoRepository.GetAllAsync(request.Count, request.SortDirection);
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
                        Text = _.Description,
                        HostingLink = links.WebsiteLink,
                    };
                }).ToList()
            };
        }
    }
}