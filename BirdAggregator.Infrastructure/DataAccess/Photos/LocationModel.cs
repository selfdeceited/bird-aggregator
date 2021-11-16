using MongoDB.Bson.Serialization.Attributes;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class LocationModel
    {
        public string Neighbourhood { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string GeoTag { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Locality { get; set; }
    }
}