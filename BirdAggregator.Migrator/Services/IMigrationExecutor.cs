using System.Collections.Generic;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Services
{
    public interface IMigrationExecutor
    {
        Task<PhotoResponse.Photo> GetPhotoInfo(PhotoId photoId, CancellationToken ct);
        Task<bool> RequireDatabaseUpdate(PhotoId photoId, CancellationToken ct);
        Task<int> GetPages(CancellationToken ct);
        Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken ct);
        Task<Sizes> GetSizes(PhotoId photoId, CancellationToken ct);
        Task<Location> GetLocation(PhotoId photoId, CancellationToken ct);
        Task EnsureCollectionsExist(CancellationToken ct);
        Task<SavePhotoResult[]> SavePhotosInformation(IList<SavePhotoModel> savePhotoModels, CancellationToken ct);
        Task TrackDuplicatePhotos();
    }
}