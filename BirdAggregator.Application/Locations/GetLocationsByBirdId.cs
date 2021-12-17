using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations
{
    public record GetLocationsByBirdIdQuery(string BirdId): IQuery<LocationListDto>;
    public class GetLocationsByBirdIdQueryHandler : IQueryHandler<GetLocationsByBirdIdQuery, LocationListDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationService _locationService;
        private readonly IPhotoRepository _photoRepository;

        public GetLocationsByBirdIdQueryHandler(ILocationRepository locationRepository, ILocationService locationService, IPhotoRepository photoRepository)
        {
            _locationRepository = locationRepository;
            _locationService = locationService;
            _photoRepository = photoRepository;
        }

        public async Task<LocationListDto> Handle(GetLocationsByBirdIdQuery request, CancellationToken cancellationToken)
        {
            var photos = await _photoRepository.GetGalleryForBirdAsync(request.BirdId);
            var markers = photos
                .Select(_locationService.GetMarker)
                .Where(m => m != null);

            return new LocationListDto
            {
                Markers = markers.ToList()
            };
        }
    }
}