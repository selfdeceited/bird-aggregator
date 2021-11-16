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
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string accuracy { get; set; }
        public string context { get; set; }
        public Neighbourhood neighbourhood { get; set; }
        public Locality locality { get; set; }
        public County county { get; set; }
        public Region region { get; set; }
        public Country country { get; set; }
        public string place_id { get; set; }
        public int? woeid { get; set; }
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