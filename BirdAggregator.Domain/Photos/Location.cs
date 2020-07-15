using System;

namespace BirdAggregator.Domain.Photos
{
    public class Location
    {
        Func<string, string> addComma = s => 
                    string.IsNullOrEmpty(s) ? string.Empty : s + ",";
                    
        public string Description {
            get {
                if (!this.Specified)
                    return "unspecified location";
                return $"{addComma(this.Neighbourhood)} {addComma(this.Region)} {this.Country}";
            }
        }

        public Location(int id)
        {
            Id = id;
        }

        public bool Specified { get; set; }
        public int Id { get; }
        public string Neighbourhood { get; private set; }
        public string Region { get; private set; }
        public object Country { get; private set; }
    }
}