using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirdAggregator.Domain.Birds
{
    public interface IBirdRepository
    {
        Task<List<Bird>> GetAllAsync();
        Task<Bird> Get(int birdId);
        Task<List<Bird>> GetBirdsByIds(IEnumerable<int> birdIds);
    }
}
