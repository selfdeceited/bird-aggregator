using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.Mongo;
using BirdAggregator.Migrator.ResponseModels;
using BirdAggregator.Migrator.Repositories;
using Colorify;
using static BirdAggregator.Migrator.Program;

namespace BirdAggregator.Migrator.Services
{
    public class MigrationExecutor : IMigrationExecutor
    {
        private readonly bool testMode = false;
        private readonly IPictureFetchingService _pictureFetchingService;
        private readonly IPhotoWriteRepository _photoWriteRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMongoConnection _mongoConnection;

        public MigrationExecutor(IPictureFetchingService pictureFetchingService, IPhotoWriteRepository photoWriteRepository, IPhotoRepository photoRepository, IMongoConnection mongoConnection)
        {
            _pictureFetchingService = pictureFetchingService;
            _photoWriteRepository = photoWriteRepository;
            _photoRepository = photoRepository;
            _mongoConnection = mongoConnection;
        }

        public async Task EnsureCollectionsExist(CancellationToken ct)
        {
            if (testMode)
                await _mongoConnection.TruncateAll(ct);
            
            await _mongoConnection.BootstrapDb(ct);
            ColoredConsole.WriteLine("collections exist in db", Colors.txtPrimary);
        }

        public async Task<SavePhotoResult[]> SavePhotosInformation(IList<SavePhotoModel> savePhotoModels, CancellationToken ct)
        {
            await _photoWriteRepository.SavePhotos(savePhotoModels, ct);
            
            return savePhotoModels.Select(savePhotoModel =>
            {
                ColoredConsole.WriteLine(
                    $"        > data for photo #{savePhotoModel.photo.id} ({savePhotoModel.photo.title._content} at {savePhotoModel.photo.dates.taken}) saved",
                    Colors.txtInfo);
               return new SavePhotoResult(savePhotoModel.photo.id);
            }).ToArray();
        }

        public async Task<Location> GetLocation(PhotoId photoId, CancellationToken ct)
        {
            var locationResponse = await _pictureFetchingService.GetLocation(photoId.flickrId, ct);
            if (locationResponse == null)
            {
                ColoredConsole.WriteLine($"empty location for photo #{photoId.flickrId}", Colors.txtWarning);
                return null;
            }
            ColoredConsole.WriteLine($"        > location for #{photoId.flickrId} fetched: {locationResponse.photo.location.place_id}", Colors.txtInfo);
            return locationResponse.photo.location;
        }

        public async Task<PhotoResponse.Photo> GetPhotoInfo(PhotoId photoId, CancellationToken ct)
        {
            var photoInfo = await _pictureFetchingService.GetPhotoInfo(photoId.flickrId, ct);
            ColoredConsole.WriteLine($"        > info for #{photoId.flickrId} fetched: {photoInfo.photo.dates.taken}", Colors.txtInfo);
            return photoInfo.photo;
        }

        public async Task<bool> RequireDatabaseUpdate(PhotoId photoId, CancellationToken ct)
        {
            var (flickrId, title) = photoId;
            try
            {
                var photo = await _photoRepository.GetByHostingId(flickrId);
                if (photo == null)
                    return true;


                // ColoredConsole.WriteLine("already saved: " + JsonSerializer.Serialize(photo), Colors.txtWarning);

                // todo: detect if caption is changed and the reupload is required.
                return false;
                
            }
            catch (Exception e)
            {
                ColoredConsole.WriteLine($"{e}", Colors.bgDanger);
                return false;
            }
        }

        public async Task<int> GetPages(CancellationToken ct)
        {
            if (testMode) return await Task.FromResult(3);
            var pages = await _pictureFetchingService.GetPages(ct);
            ColoredConsole.WriteLine($"    > pages: {pages}", Colors.txtMuted);
            return pages;
        }
        
        public async Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken ct)
        {
            var photoIds = await _pictureFetchingService.GetPhotoInfoForPage(pageNumber, ct);
            if (testMode)
            {
                photoIds = photoIds.ToArray();
            }
            ColoredConsole.WriteLine($"    > data from page {pageNumber} fetched", Colors.txtMuted);
            return photoIds;
        }

        public async Task<Sizes> GetSizes(PhotoId photoId, CancellationToken ct)
        {
            var response = await _pictureFetchingService.GetSize(photoId.flickrId, ct);
            ColoredConsole.WriteLine($"        > sizes for #{photoId.flickrId} fetched: {response.sizes.size.FirstOrDefault()?.url}", Colors.txtInfo);
            return response.sizes;
        }

        public async Task TrackDuplicatePhotos()
        {
            await _photoWriteRepository.TrackDuplicatePhotos();
        }
    }
}