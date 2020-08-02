using BirdAggregator.Domain.Birds;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BirdAggregator.Infrastructure.DataAccess.Birds
{
    public class BirdRepository: IBirdRepository {
        public async Task<Bird> Get(int birdId)
        {
            var allBirds = await GetAllAsync();
            return allBirds.SingleOrDefault(bird => bird.Id == birdId);
        }

        public async Task<List<Bird>> GetAllAsync() {
            var fileContent = await File.ReadAllTextAsync(@"../data/data.birds.json");
            var birdsModel = JsonConvert.DeserializeObject<List<BirdModel>>(fileContent);

            return birdsModel.Select(model => new Bird(model.Id, model.Name)).ToList();
        }

        public async Task<List<Bird>> GetBirdsByIds(IEnumerable<int> birdIds)
        {
            var allBirds = await GetAllAsync();
            return allBirds.Where(bird => birdIds.Contains(bird.Id)).ToList();
        }
    }
}