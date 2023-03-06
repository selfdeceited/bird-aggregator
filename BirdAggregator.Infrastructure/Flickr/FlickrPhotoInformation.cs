using System.Collections.Generic;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Infrastructure.Flickr
{
    public class FlickrPhotoInformation : IPhotoInformation
    {
        private readonly string _id;
        private readonly Dictionary<string, string> _metadata;
        public FlickrPhotoInformation(string flickrId, int farmId, string serverId, string secret)
        {
            _id = flickrId;
            _metadata = new Dictionary<string, string> {
                {"FarmId", farmId.ToString()},
                {"ServerId", serverId},
                {"Secret", secret},
            };
        }

        public string Id => _id;
        public Dictionary<string, string> Metadata => _metadata;
    }
}