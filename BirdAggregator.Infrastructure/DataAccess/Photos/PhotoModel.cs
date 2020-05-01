using System;
using System.Collections.Generic;

namespace BirdAggregator.Infrastructure.DataAccess.Birds
{
    internal class PhotoModel
    {
        public int Id { get; set; }
        public IEnumerable<int> BirdIds {get; set; }
        public string FlickrId { get; set; }
        public int LocationId {get; set;}
        public int FarmId { get; set; }
        public string ServerId { get; set; }
        public string Secret { get; set; }
        public DateTime DateTaken { get; set; }
        public string Description { get; set; }
        public double Ratio { get; set; }
    }
}