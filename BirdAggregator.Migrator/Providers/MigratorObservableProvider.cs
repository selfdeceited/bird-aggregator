using System;
using System.Reactive;
using System.Reactive.Linq;
using BirdAggregator.Migrator.ResponseModels;
using BirdAggregator.Migrator.Services;

namespace BirdAggregator.Migrator.Providers
{
    public class MigratorObservableProvider : IMigratorObservableProvider
    {
        private readonly IMigrationExecutor _m;

        public MigratorObservableProvider(IMigrationExecutor migrationExecutor)
        {
            _m = migrationExecutor;
        }
        public IObservable<int> GetPages() => Observable
            .FromAsync(_m.GetPages)
            .SelectMany(p => Observable.Range(0, p));
        
        public IObservable<Unit> SavePhoto((PhotoResponse.Photo photo, Sizes sizes) _)
            => Observable.FromAsync(ct => _m.SavePhotoInformation(_.photo, _.sizes, ct));

        public IObservable<bool> ShouldUpdateDb(PhotoId photoId)
            => Observable
                .FromAsync(ct => _m.RequireDatabaseUpdate(photoId, ct));
        
        public IObservable<PhotoId> GetPhotoId(int pageNumber)
            => Observable
                .FromAsync(ct => _m.GetPhotoInfoForPage(pageNumber, ct))
                .SelectMany(o => o);
            
        private IObservable<PhotoResponse.Photo> GetPhoto(PhotoId x)
            => Observable.FromAsync(ct => _m.GetPhotoInfo(x, ct));

        private IObservable<Sizes> GetSizes(PhotoId x)
            => Observable.FromAsync(ct => _m.GetSizes(x, ct));

        public IObservable<(PhotoResponse.Photo photo, Sizes sizes)> GetPhotoWithSizesByPhotoId(PhotoId x)
            => GetPhoto(x).Zip(GetSizes(x), (photo, size) => (photo, size));
    }
}