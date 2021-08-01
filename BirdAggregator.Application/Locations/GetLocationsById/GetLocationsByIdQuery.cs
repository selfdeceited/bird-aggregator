using BirdAggregator.Application.Locations.GetLocations;

namespace BirdAggregator.Application.Locations.GetLocationsById
{
    public class GetLocationsByIdQuery: IQuery<LocationListDto>
    {
        public int PhotoId { get;}

        public GetLocationsByIdQuery(int id)
        {
            PhotoId = id;
        }
    }
}