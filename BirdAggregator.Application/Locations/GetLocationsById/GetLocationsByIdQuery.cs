using BirdAggregator.Application.Locations.GetLocations;

namespace BirdAggregator.Application.Locations.GetLocationsById
{
    public class GetLocationsByIdQuery: IQuery<LocationListDto>
    {
        public string PhotoId { get;}

        public GetLocationsByIdQuery(string id)
        {
            PhotoId = id;
        }
    }
}