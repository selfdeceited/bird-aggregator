using System.Reactive;
using System.Reactive.Linq;
using BirdAggregator.Migrator.Services;

namespace BirdAggregator.Migrator.Providers;

public class MigratorObservableProvider(IMigrationExecutor migrationExecutor) : IMigratorObservableProvider
{
    public IObservable<Unit> EnsureCollectionsExist() => Observable.FromAsync(migrationExecutor.EnsureCollectionsExist);
    public IObservable<SavePhotoResult[]> SavePhotos(IList<SavePhotoModel> savePhotoModels)
    {
        return Observable.FromAsync(ct => migrationExecutor.SavePhotosInformation(savePhotoModels, ct));
    }

    public IObservable<int> GetPages() => Observable
        .FromAsync(migrationExecutor.GetPages)
        .SelectMany(p => Observable.Range(0, p));

    public IObservable<bool> ShouldUpdateDb(PhotoId photoId)
        => Observable
            .FromAsync(ct => migrationExecutor.RequireDatabaseUpdate(photoId, ct));

    public IObservable<PhotoId> GetPhotoId(int pageNumber)
        => Observable
            .FromAsync(ct => migrationExecutor.GetPhotoInfoForPage(pageNumber, ct))
            .SelectMany(o => o);


    public IObservable<SavePhotoModel> GetPhotoInfoForSave(PhotoId x)
    {
        return Observable.FromAsync(async (ct) =>
        {
            var photoTask = migrationExecutor.GetPhotoInfo(x, ct);
            var locationTask = migrationExecutor.GetLocation(x, ct);
            var sizesTask = migrationExecutor.GetSizes(x, ct);
            await Task.WhenAll(photoTask, locationTask, sizesTask);
            var model = new SavePhotoModel(photoTask.Result, locationTask.Result, sizesTask.Result);
            return model;
        });
    }

    public Task TrackDuplicatePhotos(CancellationToken ct)
    {
        return migrationExecutor.TrackDuplicatePhotos(ct);
    }
}
