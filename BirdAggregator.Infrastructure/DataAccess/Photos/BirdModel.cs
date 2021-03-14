using MongoDB.Bson.Serialization.Attributes;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    internal class BirdModel
    {
        [BsonId]
        public int Id { get; set; }
        public string LatinName { get; set; }
        public string EnglishName { get; set; }
    }
}
