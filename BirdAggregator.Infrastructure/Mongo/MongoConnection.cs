using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using MongoDB.Driver;
using Newtonsoft.Json;

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

            // await InitTestDatabase();
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

        private Task InitTestDatabase()
        {
            if (!_appSettings.IsTestRun)
                return Task.CompletedTask;
            var initTasks = new[] {
                InitCollection<PhotoModel>(@"../data/data.photos.json", "photos"),
                InitCollection<BirdModel>(@"../data/data.birds.json", "birds")
            };

            return Task.WhenAll(initTasks);

        }

        private async Task InitCollection<T>(string fileName, string collectionName)
        {
            var collection = Database.GetCollection<T>(collectionName);
            if (await collection.AsQueryable().AnyAsync())
                return;

            var path = Path.Combine(Assembly.GetEntryAssembly()?.Location ?? "", "../", fileName);
            Console.WriteLine(path);
            var fileContent = await File.ReadAllTextAsync(path);
            var models = JsonConvert.DeserializeObject<List<T>>(fileContent);
            await collection.InsertManyAsync(models);
        }
    }
}