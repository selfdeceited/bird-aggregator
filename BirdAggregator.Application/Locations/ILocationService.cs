using System.Threading.Tasks;
using BirdAggregator.Application.Locations.GetLocations;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Application.Locations
{
    public interface ILocationService
    {
        Task<MarkerDto> GetAsync(Location location);
    }
}