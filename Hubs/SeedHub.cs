using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using birds.POCOs;
using birds.Services;
using Microsoft.AspNetCore.SignalR;

namespace bird_aggregator.Hubs
{
    public class SeedHub: Hub
    {
        private readonly SeedLauncher _seedLauncher;
        private readonly SeedService _seedService;

        public SeedHub(SeedLauncher seedLauncher, SeedService seedService)
        {
            _seedLauncher = seedLauncher;
            _seedService = seedService;
        }

        public async Task Launch()
        {
            await _seedLauncher.WrapLaunch(async () => {
                _seedService.TruncateDb();
                await _seedLauncher.Log("DB truncated");

                var count = _seedService.GetPageCount();
                var metadata = _seedService.GetMetaData(count);
                await _seedLauncher.Log("Metadata received");

                await _seedLauncher.BroadcastCount(metadata.Count());

                await LoadBirds(metadata);
                await LoadPhotos(metadata);
                await _seedLauncher.Log("Completed");
            }, _seedService.AnythingSaved());
        }

        private async Task LoadPhotos(IEnumerable<PhotosResponse.Photo> metadata)
        {
            await _seedLauncher.Log("Photo loading begins ...");
            foreach (var photo in metadata)
            {
                if (await _seedService.SavePhotoAsync(photo))
                    await _seedLauncher.PhotoSaved();
            };
        }

        private async Task LoadBirds(IEnumerable<PhotosResponse.Photo> metadata){
            await _seedLauncher.Log("Bird loading ...");
            var namesToExtract = metadata.Select(x => x.title.Substring("B: ".Length));
            var birdNames = _seedService.ExtractBirdNames(namesToExtract);

            var birdCount = birdNames.Contains("undefined") ? birdNames.Count() - 1 : birdNames.Count();
            await _seedLauncher.BroadcastBirdCount(birdCount);

            foreach (var bird in birdNames)
            {
                if (await _seedService.SaveBirdAsync(bird))
                    await _seedLauncher.BirdSaved();
            };
        }
    }
}