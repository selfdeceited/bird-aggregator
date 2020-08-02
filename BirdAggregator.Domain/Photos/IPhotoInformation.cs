using System.Collections.Generic;

namespace BirdAggregator.Domain.Photos
{
    public interface IPhotoInformation
    {
        string Id { get; }
        Dictionary<string, string> Metadata { get; }
    }
}