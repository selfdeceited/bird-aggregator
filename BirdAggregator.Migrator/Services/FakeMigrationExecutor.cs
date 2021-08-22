using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Services
{
    public class FakeMigrationExecutor: IMigrationExecutor
    {
        private Random _random;

        public FakeMigrationExecutor()
        {
            _random = new Random();
        }
        
        public async Task SavePhotoInformation(PhotoResponse.Photo photo, CancellationToken ct)
        {
            await Task.Delay(50, ct);
        }

        public async Task<PhotoResponse.Photo> GetPhotoInfo(PhotoId photoId, CancellationToken ct)
        {
            await Task.Delay(50, ct);
            return new PhotoResponse.Photo
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

        public async Task<bool> RequireDatabaseUpdate(PhotoId photoId, CancellationToken ct)
        {
            await Task.Delay(50, ct);
            var x = _random.NextDouble();
            return x > 0.5;
        }

        public async Task<int> GetPages(CancellationToken ct)
        {
            await Task.Delay(50, ct);
            return 10;
        }

        public async Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken ct)
        {
            await Task.Delay(50, ct);
            var photosCount = 5;
            return Enumerable
                .Range(0, photosCount)
                .Select(x => new PhotoId($"{pageNumber}_{x}__", $"B: bird_{x}_{pageNumber}"))
                .ToArray();

        }
    }
}