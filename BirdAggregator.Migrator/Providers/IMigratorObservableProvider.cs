using System;
using System.Reactive;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Providers
{
    public interface IMigratorObservableProvider
    {
        IObservable<(PhotoResponse.Photo photo, Sizes sizes)> GetPhotoWithSizesByPhotoId(PhotoId x);
        IObservable<PhotoId> GetPhotoId(int pageNumber);
        IObservable<bool> ShouldUpdateDb(PhotoId photoId);
        IObservable<Unit> SavePhoto((PhotoResponse.Photo photo, Sizes sizes) _);
        IObservable<int> GetPages();
    }
}