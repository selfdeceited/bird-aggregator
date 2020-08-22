using System.Threading.Tasks;
using MongoDB.Driver;

namespace BirdAggregator.Infrastructure.Mongo
{
    public interface IMongoConnection
    {
        IMongoDatabase Database { get; }
        Task BootstrapDb();
    }
}