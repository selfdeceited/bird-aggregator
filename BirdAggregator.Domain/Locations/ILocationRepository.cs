using System.Threading.Tasks;

namespace BirdAggregator.Domain.Locations
{
    public interface ILocationRepository
    {
        Task<Location[]> GetAllAsync();
        Task<Location> GetByIdAsync(int id);
    }
}