using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirdAggregator.Domain.Photos
{
    public interface IPhotoRepository
    {
        Task<List<Photo>> GetAllAsync();
        Task<List<Photo>> GetGalleryForBirdAsync(int birdId);
        Task<List<Photo>> GetAllAsync(int count);
    }
}
