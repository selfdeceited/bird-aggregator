namespace BirdAggregator.Migrator.ResponseModels
{

    public class Neighbourhood
    {
        public string _content { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
    }

    public class Locality
    {
        public string _content { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
    }

    public class County
    {
        public string _content { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
    }

    public class Region
    {
        public string _content { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
    }

    public class Country
    {
        public string _content { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int accuracy { get; set; }
        public int context { get; set; }
        public Neighbourhood neighbourhood { get; set; }
        public Locality locality { get; set; }
        public County county { get; set; }
        public Region region { get; set; }
        public Country country { get; set; }
        public string place_id { get; set; }
        public string woeid { get; set; }
    }

    public class Photo
    {
        public string id { get; set; }
        public Location location { get; set; }
    }

    public class LocationResponse: IStateResponse
    {
        public Photo photo { get; set; }
        public string stat { get; set; }
    }
}