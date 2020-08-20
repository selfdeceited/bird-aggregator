using System.Collections.Generic;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Domain.Photos
{
    public interface IPhotoRepository
    {
        Task<List<Photo>> GetAllAsync();
        Task<List<Photo>> GetGalleryForBirdAsync(int birdId);
        Task<List<Photo>> GetAllAsync(int count);
        Task<Photo> GetById(int photoId);
        Task<List<Photo>> GetByLocationAsync(int id);
        Task<IEnumerable<Location>> GetByBirdIdAsync(int birdId);
    }
}
