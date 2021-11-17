using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations.GetLocations
{
    public class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, LocationListDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ILocationService _locationService;

        public GetLocationsQueryHandler(
            ILocationRepository locationRepository,
            IPhotoRepository photoRepository,
            ILocationService locationService)
        {
            _locationRepository = locationRepository;
            _locationService = locationService;
            _photoRepository = photoRepository;
        }

        public async Task<LocationListDto> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var photos = await _photoRepository.GetAllAsync();
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