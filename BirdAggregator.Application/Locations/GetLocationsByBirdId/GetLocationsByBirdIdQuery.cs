using BirdAggregator.Application.Locations.GetLocations;

namespace BirdAggregator.Application.Locations.GetLocationsByBirdId
{
    public class GetLocationsByBirdIdQuery: IQuery<LocationListDto>
    {
        public GetLocationsByBirdIdQuery(int id)
        {
            BirdId = id;
        }

        public int BirdId { get; }
    }
}