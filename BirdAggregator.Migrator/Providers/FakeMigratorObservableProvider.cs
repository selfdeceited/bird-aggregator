using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Providers
{
    public class FakeMigratorObservableProvider: IMigratorObservableProvider
    {
        private readonly Random _random = new();
       
        public IObservable<SavePhotoModel> GetPhotoInfoForSave(PhotoId x)
        {
            var sizes = new Sizes
            {
                size = new List<Size>
                {
                    new() { height = 100, width = 100 }
                }
            };

            var photo = GetPhotoInfo(x, CancellationToken.None);
            var location = new Location();

            return Observable
                .FromAsync(ct => Task.FromResult(new SavePhotoModel(photo, location, sizes)))
                .Delay(TimeSpan.FromMilliseconds(50));
        }

        public IObservable<PhotoId> GetPhotoId(int pageNumber)
        {
            return Observable
                .FromAsync(ct => GetPhotoInfoForPage(pageNumber, ct))
                .SelectMany(x => x);
        }

        public IObservable<bool> ShouldUpdateDb(PhotoId photoId) =>
            Observable.FromAsync(() => Task.Delay(50))
                .Select(_ => _random.NextDouble() > 0.5);

        public IObservable<int> GetPages() =>
            Observable
                .FromAsync(() => Task.Delay(50))
                .SelectMany(_ => Observable.Range(0, 10));

        public IObservable<Unit> EnsureCollectionsExist()
        {
            return Observable.Empty<Unit>();
        }

        public IObservable<SavePhotoResult[]> SavePhotos(IList<SavePhotoModel> savePhotoModels)
        {
            return Observable.FromAsync(() => Task.Delay(50))
                .Select(_ => savePhotoModels
                    .Select(model => new SavePhotoResult(model.photo.id))
                    .ToArray()
                );
        }

        private PhotoResponse.Photo GetPhotoInfo(PhotoId photoId, CancellationToken ct)
        {
            return new()
            {
                id = photoId.flickrId,
                title = new PhotoResponse.Title
                {
                    _content = photoId.title
                },
                dates = new PhotoResponse.Dates
                {
                    taken = DateTime.Now.ToLongTimeString()
                }
            };
        }

        private async Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken ct)
        {
            await Task.Delay(50, ct);
            var photosCount = 5;
            return Enumerable
                .Range(0, photosCount)
                .Select(x => new PhotoId($"{pageNumber}_{x}__", $"B: bird_{x}_{pageNumber}"))
                .ToArray();

        }

        public Task TrackDuplicatePhotos()
        {
            return Task.CompletedTask;
        }
    }
}