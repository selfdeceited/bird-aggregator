using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    internal class PhotoModel
    {
        [BsonId]
        public int Id { get; set; }
        public IEnumerable<int> BirdIds { get; set; }
        public FlickrModel Flickr { get; set; }
        public LocationModel Location { get; set; }
        public DateTime DateTaken { get; set; }
        public string Description { get; set; }
        public double Ratio { get; set; }
    }
}