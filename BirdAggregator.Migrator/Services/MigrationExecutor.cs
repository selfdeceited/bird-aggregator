using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Migrator.ResponseModels;
using BirdAggregator.Migrator.Repositories;
using Colorify;

namespace BirdAggregator.Migrator.Services
{
    public class MigrationExecutor : IMigrationExecutor
    {
        private readonly IPictureFetchingService _pictureFetchingService;
        private readonly IPhotoWriteRepository _photoWriteRepository;
        private readonly IPhotoRepository _photoRepository;

        public MigrationExecutor(IPictureFetchingService pictureFetchingService, IPhotoWriteRepository photoWriteRepository, IPhotoRepository photoRepository)
        {
            _pictureFetchingService = pictureFetchingService;
            _photoWriteRepository = photoWriteRepository;
            _photoRepository = photoRepository;
        }
        
        public async Task SavePhotoInformation(PhotoResponse.Photo photo, Sizes sizes, CancellationToken ct)
        {
            await _photoWriteRepository.SavePhoto(photo, sizes, ct);
            Program.ColoredConsole.WriteLine($"        > data for photo {photo.title} ({photo.dates.taken}) saved", Colors.txtPrimary);
        }

        public async Task<SavePhotoModel> GetPhotoInfo(PhotoId photoId, CancellationToken ct)
        {
            var photoInfo = await _pictureFetchingService.GetPhotoInfo(photoId.flickrId, ct);
            var location = _pictureFetchingService.GetLocation(photoInfo.id, ct);
            return new SavePhotoModel(photoInfo.photo, location.photo.location);
        }

        public async Task<bool> RequireDatabaseUpdate(PhotoId photoId, CancellationToken ct)
        {
            var (flickrId, title) = photoId;
            try
            {
                var photo = await _photoRepository.GetByHostingId(flickrId);
                Program.ColoredConsole.WriteLine(JsonSerializer.Serialize(photo), Colors.txtWarning);
                if (photo == null)
                    return true;
                return photo.Caption != title;
            }
            catch (Exception e)
            {
                Program.ColoredConsole.WriteLine($"{e}", Colors.bgDanger);
                return false;
            }
        }

        public async Task<int> GetPages(CancellationToken ct)
        {
            var pages = await _pictureFetchingService.GetPages(ct);
            Program.ColoredConsole.WriteLine($"    > pages: {pages}", Colors.txtMuted);
            return pages;
        }
        
        public async Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken ct)
        {
            var photoIds = await _pictureFetchingService.GetPhotoInfoForPage(pageNumber, ct);
            Program.ColoredConsole.WriteLine($"    > data from page {pageNumber} fetched", Colorify.Colors.txtMuted);
            return photoIds;
        }

        public async Task<Sizes> GetSizes(PhotoId photoId, CancellationToken ct)
        {
            var response = await _pictureFetchingService.GetSize(photoId.flickrId, ct);
            return response.sizes;
        }
    }
}