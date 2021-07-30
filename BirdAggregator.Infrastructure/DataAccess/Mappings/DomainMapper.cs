namespace BirdAggregator.Infrastructure.DataAccess.Mappings
{
    internal abstract class DomainMapper<TModel, TDomain>
    {
        public abstract TDomain ToDomain(TModel model);
    }
}