using System;
using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Domain.Locations
{
    public class Location: IAggregateRoot
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

        public Location()
        {
            Specified = false;
        }

        public Location(int id, string country, string neighbourhood, string region, double x, double y)
        {
            Id = id;
            Country = country;
            Neighbourhood = neighbourhood;
            Region = region;
            Latitude = y;
            Longitude = x;
            Specified = true;
        }

        public bool Specified { get; set; }
        public int Id { get; }
        public string Neighbourhood { get; private set; }
        public string Region { get; private set; }
        public string Country { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
    }
}