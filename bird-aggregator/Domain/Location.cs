namespace birds.Domain
{
    public class Location
    {
        public int Id { get; set; }
        public string Neighbourhood { get; set; }
        public string Region { get; set; }
        public string GeoTag { get; set; }
        public string Country { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
    }
}