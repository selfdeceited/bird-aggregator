namespace BirdAggregator.Application.Configuration.Data
{
    public interface IConnectionFactory
    {
        IConnection GetOpenConnection();
    }
    
    public interface IConnection{

    }
}