using BirdAggregator.Domain.Locations;
using BirdAggregator.Infrastructure.DataAccess.Photos;

namespace BirdAggregator.Infrastructure.DataAccess.Mappings
{
    internal class LocationMapper: DomainMapper<LocationModel, Location>
    {
        public override Location ToDomain(LocationModel model)
        {
            return new(
                model.Country,
                model.Neighbourhood,
                model.Region,
                model.X,
                model.Y,
                model.Locality
            );
        }
    }
}