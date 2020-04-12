using System;
using BirdAggregator.Domain.Birds;

namespace BirdAggregator.Domain
{
    public class Photo
    {
        public int Id { get; set; }

        public Bird[] Birds {get; set; }

        public Location Location {get; set;}
        public DateTime DateTaken { get; set; }
        public string Description { get; set; }
        public double Ratio { get; set; }
    }
}