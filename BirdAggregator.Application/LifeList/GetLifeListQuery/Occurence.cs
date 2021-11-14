using System;

namespace BirdAggregator.Application.LifeList.GetLifeListQuery
{
    public class Occurence
    {
        public string BirdId { get; set; }
        public string Name { get; set; }
        public DateTime DateMet { get; set; }
        public string Location { get; set; }
        public string PhotoId { get; set; }
    }
}