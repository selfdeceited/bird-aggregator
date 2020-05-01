namespace BirdAggregator.Domain.Photos
{
    public class FlickrPhoto
    {
        public string FlickrId { get; }
        public int FarmId { get; }
        public string ServerId { get; }
        public string Secret { get; }

        public FlickrPhoto(string flickrId, int farmId, string serverId, string secret)
        {
            FlickrId = flickrId;
            FarmId = farmId;
            ServerId = serverId;
            Secret = secret;
        }
    }
}