using BirdAggregator.Application.Locations.GetLocations;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations
{
    public interface ILocationService
    {
        MarkerDto GetMarker(Photo photo);
    }
}