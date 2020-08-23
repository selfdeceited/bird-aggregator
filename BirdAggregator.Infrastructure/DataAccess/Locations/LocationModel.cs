using MongoDB.Bson.Serialization.Attributes;

namespace BirdAggregator.Infrastructure.DataAccess.Locations
{
    public class LocationModel
    {
        [BsonId]
        public int Id { get; set; }
        public string Neighbourhood { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}