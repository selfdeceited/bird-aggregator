using MongoDB.Driver;

namespace BirdAggregator.Infrastructure.Mongo;

public interface IMongoConnection
{
    IMongoDatabase Database { get; }
    Task BootstrapDb(CancellationToken cancellationToken);
    Task TruncateAll(CancellationToken cancellationToken);
    Task<T> ExecuteInTransaction<T>(Func<IClientSessionHandle, CancellationToken, Task<T>> execute,
        CancellationToken cancellationToken);
}
