using System.Collections.Generic;

namespace BirdAggregator.Domain.Photos
{
    public interface IBirdInfo
    {
        string Description { get; set; }
        string ImageLink { get; set; }
    }
}