using BirdAggregator.Domain.Birds;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BirdAggregator.Infrastructure.DataAccess.Birds
{
    public class BirdRepository: IBirdRepository {
        public async Task<List<Bird>> GetAllAsync() {
            var fileContent = File.ReadAllText(@"../data/data.birds.json");
            var birdsModel = JsonConvert.DeserializeObject<List<BirdModel>>(fileContent);

            return birdsModel.Select(model => new Bird(model.Id, model.Name)).ToList();
        }
    }
}