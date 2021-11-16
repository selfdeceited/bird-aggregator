using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator.Repositories
{
    public interface IPhotoWriteRepository
    {
        Task SavePhoto(SavePhotoModel savePhotoModel, CancellationToken ct);
    }
}