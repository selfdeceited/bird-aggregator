using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirdAggregator.Domain.Birds
{
    public interface IBirdRepository
    {
        Task<Bird[]> GetAll();
        Task<Bird> Get(string birdId);
        Task<Bird[]> GetBirdsByIds(IEnumerable<string> birdIds);
    }
}
