using System.Collections.Generic;

namespace BirdAggregator.Application.Locations.GetLocations
{
    public class LocationListDto
    {
        public List<MarkerDto> Markers {get;set;}
    }

    public class MarkerDto
    {
        public int Id { get;set; }
        public double X { get;set; }
        public double Y { get;set; }
        public BirdMarkerDto[] Birds { get;set; }
        public string FirstPhotoUrl { get;set; }
    }

    public class BirdMarkerDto
    {
        public int Id {get; set;}
        public string Name {get; set;}
    }
}