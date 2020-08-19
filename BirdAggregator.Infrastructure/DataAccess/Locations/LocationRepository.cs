using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;
using Newtonsoft.Json;

namespace BirdAggregator.Infrastructure.DataAccess.Locations
{
    public class LocationRepository: ILocationRepository
    {
        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            var fileContent = await File.ReadAllTextAsync(@"../data/data.locations.json");
            var locationModels = JsonConvert.DeserializeObject<List<LocationModel>>(fileContent);
            return locationModels.Select(x =>
                new Location(x.Id, x.Country, x.Neighbourhood, x.Region, x.X, x.Y));
        }

        public async Task<IEnumerable<Location>> GetByBirdIdAsync(int birdId)
        {
            var allLocations = await GetAllAsync();
            // TODO!!!11
            return allLocations.Where(x=>true);
        }

        public async Task<Location> GetByIdAsync(int id)
        {
            var allLocations = await GetAllAsync();
            return allLocations.SingleOrDefault(x => x.Id == id);
        }
    }
}
