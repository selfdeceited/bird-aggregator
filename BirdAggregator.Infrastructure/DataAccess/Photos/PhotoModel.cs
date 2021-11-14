using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class PhotoModel
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }
        public IEnumerable<ObjectId> BirdIds { get; set; }
        public FlickrModel Flickr { get; set; }
        public LocationModel Location { get; set; }
        public DateTime DateTaken { get; set; }
        public string Description { get; set; }
        public double Ratio { get; set; }
    }
}