using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using System.Collections.Generic;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Application.Photos
{
    public record GetWebsiteLinkForPhotoQuery(string BirdId) : IQuery<GetWebsiteLinkForPhotoDto>;
    public record GetWebsiteLinkForPhotoDto(List<string> Links);
    public class GetWebsiteLinkForPhotoQueryHandler : IQueryHandler<GetWebsiteLinkForPhotoQuery, GetWebsiteLinkForPhotoDto>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPictureHostingService _pictureHostingService;
        public GetWebsiteLinkForPhotoQueryHandler(IPhotoRepository photoRepository, IPictureHostingService pictureHostingService)
        {
            _photoRepository = photoRepository;
            _pictureHostingService = pictureHostingService;
        }

        public async Task<GetWebsiteLinkForPhotoDto> Handle(GetWebsiteLinkForPhotoQuery request, CancellationToken cancellationToken)
        {
            var gallery = await _photoRepository.GetGalleryForBirdAsync(request.BirdId, SortDirection.Latest);

            var links = gallery
                .Select(_ => _pictureHostingService.GetAllImageLinks(_.PhotoInformation).WebsiteLink)
                .ToList();

            return new GetWebsiteLinkForPhotoDto(links);
        }
    }
}