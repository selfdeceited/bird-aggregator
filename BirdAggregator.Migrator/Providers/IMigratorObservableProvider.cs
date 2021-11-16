using System;
using System.Reactive;

namespace BirdAggregator.Migrator.Providers
{
    public interface IMigratorObservableProvider
    {
        IObservable<SavePhotoModel> GetPhotoInfoForSave(PhotoId x);
        IObservable<PhotoId> GetPhotoId(int pageNumber);
        IObservable<bool> ShouldUpdateDb(PhotoId photoId);
        IObservable<SavePhotoResult> SavePhoto(SavePhotoModel _);
        IObservable<int> GetPages();
        IObservable<Unit> EnsureCollectionsExist();
    }
}