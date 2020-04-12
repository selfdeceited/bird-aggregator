using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirdAggregator.Domain.Birds
{
    public interface IBirdRepository
    {
        Task<List<Bird>> GetAllAsync();
    }
}
