using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    internal class PhotoResultModel
    {
        public ObjectId Id { get; set; }
        public FlickrModel Flickr { get; set; }
        public LocationModel Location { get; set; }
        public DateTime DateTaken { get; set; }
        public string Description { get; set; }
        public double Ratio { get; set; }
        public IEnumerable<BirdModel> BirdModels { get; set; }
        public IEnumerable<ObjectId> BirdIds { get; set; }
    }
}