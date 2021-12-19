using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public IObservable<Unit> EnsureCollectionsExist() => Observable.FromAsync(_m.EnsureCollectionsExist);
        public IObservable<SavePhotoResult[]> SavePhotos(IList<SavePhotoModel> savePhotoModels)
        {
            return Observable.FromAsync(ct => _m.SavePhotosInformation(savePhotoModels, ct));
        }

        public IObservable<int> GetPages() => Observable
            .FromAsync(_m.GetPages)
            .SelectMany(p => Observable.Range(0, p));

        public IObservable<bool> ShouldUpdateDb(PhotoId photoId)
            => Observable
                .FromAsync(ct => _m.RequireDatabaseUpdate(photoId, ct));
        
        public IObservable<PhotoId> GetPhotoId(int pageNumber)
            => Observable
                .FromAsync(ct => _m.GetPhotoInfoForPage(pageNumber, ct))
                .SelectMany(o => o);
        

        public IObservable<SavePhotoModel> GetPhotoInfoForSave(PhotoId x)
        {
            return Observable.FromAsync(async (ct) =>
            {
                var photoTask = _m.GetPhotoInfo(x, ct);
                var locationTask = _m.GetLocation(x, ct);
                var sizesTask = _m.GetSizes(x, ct);
                await Task.WhenAll(photoTask, locationTask, sizesTask);  
                var model = new SavePhotoModel(photoTask.Result, locationTask.Result, sizesTask.Result);
                return model;
            });
        }

        public Task TrackDuplicatePhotos(CancellationToken ct)
        {
            return _m.TrackDuplicatePhotos(ct);
        }
    }
}