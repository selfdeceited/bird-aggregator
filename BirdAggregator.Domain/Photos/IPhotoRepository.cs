using System.Threading.Tasks;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Domain.Photos
{
    public interface IPhotoRepository
    {
        Task<Photo[]> GetAllAsync();
        Task<Photo[]> GetGalleryForBirdAsync(string birdId, SortDirection sortDirection);
        Task<Photo[]> GetAllAsync(int count, SortDirection sortDirection);
        Task<Photo> GetById(string photoId);
        Task<Photo[]> GetByLocationAsync(int id);
        Task<Photo> GetByHostingId(string hostingId);
    }
}
