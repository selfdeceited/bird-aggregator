using BirdAggregator.Application.Locations.GetLocations;

namespace BirdAggregator.Application.Locations.GetLocationsByBirdId
{
    public class GetLocationsByBirdIdQuery: IQuery<LocationListDto>
    {
        public GetLocationsByBirdIdQuery(string id)
        {
            BirdId = id;
        }

        public string BirdId { get; }
    }
}