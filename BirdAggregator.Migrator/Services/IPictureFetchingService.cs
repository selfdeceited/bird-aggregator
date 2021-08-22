using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Services
{
    public interface IPictureFetchingService
    {
        Task<int> GetPages(CancellationToken cancellationToken);
        Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken cancellationToken);
        Task<PhotoResponse> GetPhotoInfo(string hostingId, CancellationToken cancellationToken);
    }
}