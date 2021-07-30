using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirdAggregator.Domain.Birds
{
    public interface IBirdRepository
    {
        Task<Bird[]> GetAll();
        Task<Bird> Get(int birdId);
        Task<Bird[]> GetBirdsByIds(IEnumerable<int> birdIds);
    }
}
