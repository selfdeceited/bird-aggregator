using System;
using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Domain.Locations
{
    public class Location : IAggregateRoot
    {
        private readonly Func<string, string> _addComma = s =>
                    string.IsNullOrEmpty(s) ? string.Empty : s + ",";

        public string Description
            => !Specified
                ? "unspecified location"
                : $"{_addComma(Neighbourhood)} {_addComma(Region)} {Country}";

        public Location(string country, string neighbourhood, string region, double x, double y, string locality)
        {
            Country = country;
            Neighbourhood = neighbourhood;
            Region = region;
            Latitude = y;
            Longitude = x;
            Specified = true;
            Locality = locality;
        }

        public string Locality { get; }

        public bool Specified { get; }
        public string Neighbourhood { get; }
        public string Region { get; }
        public string Country { get; }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}