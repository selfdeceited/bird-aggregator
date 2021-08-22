using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Repositories
{
    public class PhotoWriteRepository : IPhotoWriteRepository
    {
        public Task SavePhoto(PhotoResponse.Photo photo, Size size, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }
    }
}