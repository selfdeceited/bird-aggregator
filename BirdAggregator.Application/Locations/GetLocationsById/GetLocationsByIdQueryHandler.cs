using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application.Locations.GetLocations;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Application.Locations.GetLocationsById
{
    public class GetLocationsByIdQueryHandler : IQueryHandler<GetLocationsByIdQuery, LocationListDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationService _locationService;

        public GetLocationsByIdQueryHandler(ILocationRepository locationRepository, ILocationService locationService)
        {
            _locationRepository = locationRepository;
            _locationService = locationService;
        }

        public async Task<LocationListDto> Handle(GetLocationsByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _locationRepository.GetByIdAsync(request.LocationId);
            var mapMarker = await _locationService.GetAsync(location);
            return new LocationListDto {
                Markers = new List<MarkerDto> {
                    mapMarker
                }
            };
        }
    }
}