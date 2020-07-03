using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Infrastructure.Wikipedia
{
    public class BirdInfo : IBirdInfo
    {
        public string Description { get; set; }
        public string ImageLink { get; set; }
    }
}