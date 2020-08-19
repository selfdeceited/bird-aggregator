using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application.Locations.GetLocations;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Application.Locations.GetLocationsByBirdId
{
    public class GetLocationsByBirdIdQueryHandler: IQueryHandler<GetLocationsByBirdIdQuery, LocationListDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationService _locationService;

        public GetLocationsByBirdIdQueryHandler(ILocationRepository locationRepository, ILocationService locationService)
        {
            _locationRepository = locationRepository;
            _locationService = locationService;
        }

        public async Task<LocationListDto> Handle(GetLocationsByBirdIdQuery request, CancellationToken cancellationToken)
        {
            var locations = await _locationRepository.GetByBirdIdAsync(request.BirdId);
            var markerDtos = await Task.WhenAll(locations.Select(_locationService.GetAsync));

            return new LocationListDto
            {
                Markers = markerDtos.ToList()
            };
        }
    }
}