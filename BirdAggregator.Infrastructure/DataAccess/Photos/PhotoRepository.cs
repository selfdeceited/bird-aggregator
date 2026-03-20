using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.DataAccess.Mappings;
using BirdAggregator.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Bson;
using SortDirection = BirdAggregator.SharedKernel.SortDirection;

namespace BirdAggregator.Infrastructure.DataAccess.Photos;

public class PhotoRepository(IMongoConnection mongoConnection) : IPhotoRepository, IBirdRepository, ILocationRepository
{
    private IMongoCollection<PhotoModel> Photos => mongoConnection.Database.GetCollection<PhotoModel>("photos");
    private IMongoCollection<BirdModel> Birds => mongoConnection.Database.GetCollection<BirdModel>("birds");

    private readonly BirdMapper _birdMapper = new();
    private readonly PhotoMapper _photoMapper = new();
    private readonly LocationMapper _locationMapper = new();

    public async Task<Bird> Get(string birdId)
    {
        var birdCursor = await Birds.FindAsync(_ => _.Id == new ObjectId(birdId));
        var birdModel = await birdCursor.SingleAsync();
        return _birdMapper.ToDomain(birdModel);
    }

    public async Task<Bird[]> GetAll()
    {
        var birdCursor = await Birds.FindAsync(_ => true);
        var birdModels = await birdCursor.ToListAsync();
        return [.. birdModels.Select(_birdMapper.ToDomain)];
    }
    public async Task<Bird[]> GetBirdsByIds(IEnumerable<string> birdIds)
    {
        var birds = birdIds.Select(x => new ObjectId(x));
        var birdCursor = await Birds.FindAsync(_ => birds.Contains(_.Id));
        var birdModels = await birdCursor.ToListAsync();
        return [.. birdModels.Select(_birdMapper.ToDomain)];
    }

    public async Task<Location[]> GetAllAsync()
    {
        var locations = await GetPhotoResultModelLookup()
            .Project(x => new { x.Location })
            .ToListAsync();

        return [.. locations.Select(_ => _locationMapper.ToDomain(_.Location))];
    }

    async Task<Photo[]> IPhotoRepository.GetAllAsync()
    {
        var photoAggregate = await GetPhotoResultModelLookup()
            .ToListAsync();

        return [.. photoAggregate.Select(_photoMapper.ToDomain)];
    }

    public async Task<Photo[]> GetAllAsync(int count, SortDirection sortDirection)
    {
        var lookup = GetPhotoResultModelLookup();

        var sortedLookup = sortDirection switch
        {
            SortDirection.Latest => lookup.SortByDescending(m => m.DateTaken),
            SortDirection.Oldest => lookup.SortBy(m => m.DateTaken),
            _ => throw new ArgumentException(null, nameof(sortDirection))
        };

        var photoResultModels = await sortedLookup
            .Limit(count)
            .ToListAsync();

        return [.. photoResultModels.Select(_photoMapper.ToDomain)];
    }


    async Task<Photo> IPhotoRepository.GetById(string photoId)
    {
        var photoAggregate = await GetPhotoResultModelLookup()
            .Match(x => x.Id == new ObjectId(photoId))
            .ToListAsync();


        var photoResultModel = photoAggregate.Single();
        return _photoMapper.ToDomain(photoResultModel);
    }

    async Task<Photo[]> IPhotoRepository.GetGalleryForBirdAsync(string birdId, SortDirection sortDirection)
    {
        var photoLookup = GetPhotoResultModelLookup()
            .Match(x => x.BirdIds.Contains(new ObjectId(birdId)));


        var sortedLookup = sortDirection switch
        {
            SortDirection.Latest => photoLookup.SortByDescending(m => m.DateTaken),
            SortDirection.Oldest => photoLookup.SortBy(m => m.DateTaken),
            _ => throw new ArgumentException(null, nameof(sortDirection))
        };

        var photoAggregate = await sortedLookup.ToListAsync();

        return [.. photoAggregate.Select(_photoMapper.ToDomain)];
    }

    private IAggregateFluent<PhotoResultModel> GetPhotoResultModelLookup()
    {
        return Photos
            .Aggregate()
            .Lookup<PhotoModel, BirdModel, PhotoResultModel>(
                Birds,
                x => x.BirdIds,
                x => x.Id,
                x => x.BirdModels);
    }

    // todo: consider switching to /map/photo/{id}
    public Task<Location> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    Task<Photo[]> IPhotoRepository.GetByLocationAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Photo> GetByHostingId(string hostingId)
    {
        var model = await GetPhotoResultModelLookup()
             .Match(x => x.Flickr.Id == hostingId)
             .FirstOrDefaultAsync();
        return model == null ? null : _photoMapper.ToDomain(model);
    }
}
