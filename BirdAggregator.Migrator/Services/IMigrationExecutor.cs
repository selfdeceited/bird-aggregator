using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Services
{
    public interface IMigrationExecutor
    {
        Task SavePhotoInformation(PhotoResponse.Photo photo, Sizes sizes, CancellationToken ct);
        Task<PhotoResponse.Photo> GetPhotoInfo(PhotoId photoId, CancellationToken ct);
        Task<bool> RequireDatabaseUpdate(PhotoId photoId, CancellationToken ct);
        Task<int> GetPages(CancellationToken ct);
        Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken ct);
        Task<Sizes> GetSizes(PhotoId photoId, CancellationToken ct);
    }
}