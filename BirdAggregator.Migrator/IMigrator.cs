using System.Threading.Tasks;

namespace BirdAggregator.Migrator
{
    public interface IMigrator
    {
        Task Run();
    }
}