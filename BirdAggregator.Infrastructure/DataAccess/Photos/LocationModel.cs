using MongoDB.Bson.Serialization.Attributes;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class LocationModel
    {
        [BsonId]
        public int Id { get; set; }
        public string Neighbourhood { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string GeoTag { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}