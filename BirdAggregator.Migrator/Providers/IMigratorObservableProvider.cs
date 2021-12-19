using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace BirdAggregator.Migrator.Providers
{
    public interface IMigratorObservableProvider
    {
        IObservable<SavePhotoModel> GetPhotoInfoForSave(PhotoId x);
        IObservable<PhotoId> GetPhotoId(int pageNumber);
        IObservable<bool> ShouldUpdateDb(PhotoId photoId);
        IObservable<int> GetPages();
        IObservable<Unit> EnsureCollectionsExist();
        IObservable<SavePhotoResult[]> SavePhotos(IList<SavePhotoModel> savePhotoModels);
        Task TrackDuplicatePhotos(CancellationToken ct);
    }
}