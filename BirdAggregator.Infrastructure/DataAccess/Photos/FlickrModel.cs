namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public class FlickrModel
    {
        public string Id { get; set; }
        public int FarmId { get; set; }
        public string ServerId { get; set; }
        public string Secret { get; set; }
    }
}