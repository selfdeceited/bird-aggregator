using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class BirdModel
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }
        public string Latin { get; set; }
        public string Name { get; set; }
    }
}
