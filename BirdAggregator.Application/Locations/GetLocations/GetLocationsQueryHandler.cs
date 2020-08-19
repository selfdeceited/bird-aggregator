using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Application.Locations.GetLocations
{
    public class GetLocationsQueryHandler : IQueryHandler<GetLocationsQuery, LocationListDto>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationService _locationService;

        public GetLocationsQueryHandler(ILocationRepository locationRepository, ILocationService locationService)
        {
            _locationRepository = locationRepository;
            _locationService = locationService;
        }

        public async Task<LocationListDto> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _locationRepository.GetAllAsync();
            var markerDtos = await Task.WhenAll(locations.Select(_locationService.GetAsync));

            return new LocationListDto
            {
                Markers = markerDtos.ToList()
            };
        }
    }
}