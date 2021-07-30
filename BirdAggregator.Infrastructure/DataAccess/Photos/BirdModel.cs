using MongoDB.Bson.Serialization.Attributes;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    internal class BirdModel
    {
        [BsonId]
        public int Id { get; set; }
        public string Latin { get; set; }
        public string Name { get; set; }
    }
}
