using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Infrastructure.Mongo;
using MongoDB.Driver;

namespace BirdAggregator.Infrastructure.DataAccess.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IMongoConnection _mongoConnection;
        private IMongoCollection<LocationModel> _locations => _mongoConnection.Database.GetCollection<LocationModel>("locations");

        public LocationRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            var allLocations = await _locations.FindAsync(_ => true);
            var locationList = await allLocations.ToListAsync();
            return locationList.Select(MapModel).ToList();
        }

        public async Task<Location> GetByIdAsync(int id)
        {
            var locations = await _locations.FindAsync(location => location.Id == id);
            var locationModel = await locations.SingleOrDefaultAsync();
            return MapModel(locationModel);
        }

        internal static Location MapModel(LocationModel x) => new Location(x.Id, x.Country, x.Neighbourhood, x.Region, x.X, x.Y);
    }
}
