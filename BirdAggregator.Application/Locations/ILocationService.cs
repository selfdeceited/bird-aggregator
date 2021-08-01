using System.Collections.Generic;
using System.Threading.Tasks;
using BirdAggregator.Application.Locations.GetLocations;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations
{
    public interface ILocationService
    {
        Task<MarkerDto> GetAsync(Location location);
        MarkerDto GetMarker(Photo photo);
    }
}