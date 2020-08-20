using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirdAggregator.Domain.Locations
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync();
        Task<Location> GetByIdAsync(int id);
    }
}