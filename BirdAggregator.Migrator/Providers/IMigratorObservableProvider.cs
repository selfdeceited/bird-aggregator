using System;
using System.Collections.Generic;
using System.Reactive;

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
        // IObservable<Unit> RemoveDuplicateBirds(IEnumerable<string> birdNames);
    }
}