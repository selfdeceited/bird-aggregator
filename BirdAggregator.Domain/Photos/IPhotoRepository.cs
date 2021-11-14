using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Domain.Photos
{
    public interface IPhotoRepository
    {
        Task<Photo[]> GetAllAsync();
        Task<Photo[]> GetGalleryForBirdAsync(string birdId);
        Task<Photo[]> GetAllAsync(int count);
        Task<Photo> GetById(string photoId);
        Task<Photo[]> GetByLocationAsync(int id);
        Task<Photo> GetByHostingId(string hostingId);
    }
}
