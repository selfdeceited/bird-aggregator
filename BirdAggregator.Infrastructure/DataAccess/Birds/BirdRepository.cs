using BirdAggregator.Domain.Birds;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using BirdAggregator.Infrastructure.Mongo;
using MongoDB.Driver;

namespace BirdAggregator.Infrastructure.DataAccess.Birds
{
    public class BirdRepository : IBirdRepository
    {
        private readonly IMongoConnection _mongoConnection;
        private IMongoCollection<BirdModel> _birds => _mongoConnection.Database.GetCollection<BirdModel>("birds");

        public BirdRepository(IMongoConnection mongoConnection)
        {
            _mongoConnection = mongoConnection;
        }

        public async Task<Bird> Get(int birdId)
        {
            var birds = await _birds.FindAsync(bird => bird.Id == birdId);
            var birdModel = await birds.SingleOrDefaultAsync();
            return MapModel(birdModel);
        }

        public async Task<List<Bird>> GetAll()
        {
            var allBirds = await _birds.FindAsync(_ => true);
            var birdList = await allBirds.ToListAsync();
            return birdList.Select(MapModel).ToList();
        }

        public async Task<List<Bird>> GetBirdsByIds(IEnumerable<int> birdIds)
        {
            var birds = await _birds.FindAsync(bird => birdIds.Contains(bird.Id));
            var birdList = await birds.ToListAsync();
            return birdList.Select(MapModel).ToList();
        }

        private Bird MapModel(BirdModel model) => new Bird(model.Id, model.Name);
    }
}