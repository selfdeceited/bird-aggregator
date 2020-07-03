using System.Threading.Tasks;

namespace BirdAggregator.Domain.Photos
{
    public interface IInformationService
    {
        Task<IBirdInfo> Get(string englishName);
    }
}