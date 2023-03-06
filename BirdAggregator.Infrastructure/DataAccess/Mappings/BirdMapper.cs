using BirdAggregator.Domain.Birds;
using BirdAggregator.Infrastructure.DataAccess.Photos;

namespace BirdAggregator.Infrastructure.DataAccess.Mappings
{

    internal class BirdMapper : DomainMapper<BirdModel, Bird>
    {
        public override Bird ToDomain(BirdModel model)
        {
            return new(model.Id.ToString(), model.Latin, model.Name);
        }
    }
}