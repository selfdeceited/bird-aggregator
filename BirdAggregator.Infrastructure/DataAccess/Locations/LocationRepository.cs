using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Domain.Locations;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BirdAggregator.Infrastructure.DataAccess.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IHostingEnvironment _appEnvironment;

        public LocationRepository(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            var path = Path.Combine(_appEnvironment.ContentRootPath, @"../data/data.locations.json");
            var fileContent = await File.ReadAllTextAsync(path);
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
