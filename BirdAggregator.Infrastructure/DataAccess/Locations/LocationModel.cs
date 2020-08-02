namespace BirdAggregator.Infrastructure.DataAccess.Locations
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Neighbourhood { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}