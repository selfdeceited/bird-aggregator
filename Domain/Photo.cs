using System;
using birds.POCOs;

namespace birds.Domain
{
    public class Photo
    {
        public int Id { get; set; }
        public int BirdId {get; set; }
        public string FlickrId { get; set; }
        public Location Location { get; set; }
        public int FarmId { get; set; }
        public string ServerId { get; set; }
        public string Secret { get; set; }
        public DateTime DateTaken { get; set; }
        public string Description { get; set; }
    }
}