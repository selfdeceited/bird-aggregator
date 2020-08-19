using BirdAggregator.Application.Locations.GetLocations;

namespace BirdAggregator.Application.Locations.GetLocationsById
{
    public class GetLocationsByIdQuery: IQuery<LocationListDto>
    {
        public int LocationId { get;}

        public GetLocationsByIdQuery(int id)
        {
            LocationId = id;
        }
    }
}