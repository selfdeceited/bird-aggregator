using System.Collections.Generic;

namespace BirdAggregator.Application.Locations
{
    public class LocationListDto
    {
        public List<MarkerDto> Markers {get;set;}
    }

    public class MarkerDto
    {
        public double X { get;set; }
        public double Y { get;set; }
        public BirdMarkerDto[] Birds { get;set; }
        public string FirstPhotoUrl { get;set; }
    }

    public class BirdMarkerDto
    {
        public string Id {get; set;}
        public string Name {get; set;}
    }
}