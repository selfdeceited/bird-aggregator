using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace birds.Domain
{
    public class Photo
    {
        public int Id { get; set; }

        [NotMapped]
        public IEnumerable<int> BirdIds {get; set; }

        public string BirdIdsAsString
        {
           get => string.Join(",", BirdIds);
	       set => BirdIds = string.IsNullOrWhiteSpace(value)
		        ? new List<int>()
		        : value.Split(',').Select(int.Parse).ToList();
        }
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