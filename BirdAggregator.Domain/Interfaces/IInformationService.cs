using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Domain.Interfaces
{
    public interface IInformationService
    {
        Task<IBirdInfo> Get(string englishName);
    }
}