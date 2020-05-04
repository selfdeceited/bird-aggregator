using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Photos.GetWebsiteLinkForPhotoQuery
{
    public class GetWebsiteLinkForPhotoQueryHanlder: IQueryHandler<GetWebsiteLinkForPhotoQuery, GetWebsiteLinkForPhotoDto>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IPictureHostingService _pictureHostingService;
        public GetWebsiteLinkForPhotoQueryHanlder(IPhotoRepository photoRepository, IPictureHostingService pictureHostingService)
        {
            _photoRepository = photoRepository;
            _pictureHostingService = pictureHostingService;
        }

        public async Task<GetWebsiteLinkForPhotoDto> Handle(GetWebsiteLinkForPhotoQuery request, CancellationToken cancellationToken)
        {
            var gallery = await _photoRepository.GetGalleryForBirdAsync(request.BirdId);
            
            return new GetWebsiteLinkForPhotoDto {
                Links = gallery
                    .Select(_ => _pictureHostingService.GetAllImageLinks(_.PhotoInformation).WebsiteLink)
                    .ToList()
             };
        }
    }
}