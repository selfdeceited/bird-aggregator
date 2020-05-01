using System;
using System.Collections.Generic;
using System.Linq;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Domain.Photos
{
    public class Photo
    {
        public Photo(int id, int locationId, string flickrId, int farmId, string serverId, IEnumerable<Bird> birds, DateTime dateTaken, double ratio, string secret, string description)
        {
            Id = id;
            Location = new Location(locationId);
            Flickr = new FlickrPhoto(flickrId, farmId, serverId, secret);
            DateTaken = dateTaken;
            Ratio = ratio;
            Description = description;
            Birds = birds;
            Url = new PhotoUrl(Flickr);
        }

        public int Id { get; }
        public PhotoUrl Url { get; }
        public string Description { get; }
        public double Ratio { get; } // todo: mb do not store and calculate?
        public DateTime DateTaken { get; }
        public Location Location { get; }
        public FlickrPhoto Flickr { get; }
        public IEnumerable<Bird> Birds { get; }
        public string Caption => string.Join(", ", Birds.Select(x => x.EnglishName));
    }
}