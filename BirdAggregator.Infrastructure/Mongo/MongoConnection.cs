using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application.Configuration;
using MongoDB.Driver;

namespace BirdAggregator.Infrastructure.Mongo
{
    public class MongoConnection : IDisposable, IMongoConnection
    {
        // todo: to config!
        public string DatabaseName => "birds";
        public IMongoDatabase Database => _database;

        private IMongoClient _client;
        private IMongoDatabase _database;
        private readonly AppSettings _appSettings;

        public MongoConnection(AppSettings appSettings)
        {
            _appSettings = appSettings;

            _client = new MongoClient(appSettings.MongoConnectionString);
            _database = _client.GetDatabase(DatabaseName);
        }

        public async Task BootstrapDb(CancellationToken ct)
        {
            var collections = await _database.ListCollectionNamesAsync(cancellationToken: ct);
            var collectionsList = await collections.ToListAsync(cancellationToken: ct);

            var dbCreationTasks = new[] { "birds", "photos" }
                .Select(_ => collectionsList.Contains(_)
                    ? Task.CompletedTask
                    : _database.CreateCollectionAsync(_, cancellationToken: ct));

            await Task.WhenAll(dbCreationTasks);
        }

        /// <summary>
        /// Warning! Do not use lightly in production, mostly for testing purposes.
        /// </summary>
        /// <param name="ct"></param>
        public async Task TruncateAll(CancellationToken ct)
        {
            var collections = await _database.ListCollectionNamesAsync(cancellationToken: ct);
            var collectionsList = await collections.ToListAsync(ct);
            var dbDropTasks = collectionsList
                .Select(_ => _database.DropCollectionAsync(_, ct));

            await Task.WhenAll(dbDropTasks);
        }

        public void Dispose()
        {
            _client = null;
            _database = null;
        }

        private async Task InitCollection<T>(string fileName, string collectionName)
        {
            var collection = Database.GetCollection<T>(collectionName);
            if (await collection.AsQueryable().AnyAsync())
                return;

            var path = Path.Combine(Assembly.GetEntryAssembly()?.Location ?? "", "../", fileName);
            Console.WriteLine(path);
            var fileContent = await File.ReadAllTextAsync(path);
            var models = JsonSerializer.Deserialize<List<T>>(fileContent);
            await collection.InsertManyAsync(models);
        }

        public async Task<T> ExecuteInTransaction<T>(Func<IClientSessionHandle, CancellationToken, Task<T>> execute,
            CancellationToken cancellationToken)
        {
            using (var session = await _client.StartSessionAsync(new ClientSessionOptions(), cancellationToken))
            {
                var transactionOptions = new TransactionOptions(
                    readPreference: ReadPreference.Primary,
                    readConcern: ReadConcern.Majority,
                    writeConcern: WriteConcern.W1);
                
                return await session.WithTransactionAsync(
                    execute,
                    transactionOptions,
                    cancellationToken);    
            }
        }
    }
}