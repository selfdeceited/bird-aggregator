namespace BirdAggregator.Infrastructure.Mongo
{
    public class MongoConnection: IDisposable, IConnection
    {
        public string DatabaseName { get; set; }
        private IMongoClient _client;
        private IMongoDatabase _database;
        private AppSettings _appSettings;

        public void MongoConnection(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _client = new MongoClient(appSettings.MongoConnectionString);
            _database = _client.GetDatabase(DatabaseName);
        }

        public void Dispose()
        {
            _client = null;
            _database = null;
        }
 
        public IMongoDatabase Database => database;
    }

    public interface IConnection
    {
        IMongoDatabase Database { get; }
    }
}