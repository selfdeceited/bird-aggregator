using System;

namespace birds.Dtos
{
    public class LifeListDto
    {
        public int BirdId { get; set; }
        public string Name { get; set; }
        public DateTime DateMet { get; set; }
        public string Location { get; set; }
        public int LocationId { get; set; }
        public int PhotoId { get; set; }
    }
}