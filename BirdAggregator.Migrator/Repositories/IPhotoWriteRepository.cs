namespace BirdAggregator.Migrator.Repositories;

public interface IPhotoWriteRepository
{
    Task SavePhotos(IList<SavePhotoModel> savePhotoModels, CancellationToken ct);
    Task TrackDuplicatePhotos(CancellationToken ct);
}
