using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Domain.Photos
{
    public interface IPhotoRepository
    {
        Task<Photo[]> GetAllAsync();
        Task<Photo[]> GetGalleryForBirdAsync(int birdId);
        Task<Photo[]> GetAllAsync(int count);
        Task<Photo> GetById(int photoId);
        Task<Photo[]> GetByLocationAsync(int id);
    }
}
