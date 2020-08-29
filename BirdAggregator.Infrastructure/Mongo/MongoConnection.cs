using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Infrastructure.DataAccess.Birds;
using BirdAggregator.Infrastructure.DataAccess.Locations;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using Microsoft.Extensions.Hosting;
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
        private AppSettings _appSettings;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MongoConnection(AppSettings appSettings, IHostingEnvironment hostingEnvironment)
        {
            _appSettings = appSettings;
            _hostingEnvironment = hostingEnvironment;

            _client = new MongoClient(appSettings.MongoConnectionString);
            _database = _client.GetDatabase(DatabaseName);
        }

        public async Task BootstrapDb()
        {
            var collections = await _database.ListCollectionNamesAsync();
            var collectionsList = await collections.ToListAsync();

            var dbCreationTasks = new[] { "birds", "photos", "locations" }.Select(_ =>
            {
                return collectionsList.Contains(_)
                  ? Task.CompletedTask
                  : _database.CreateCollectionAsync(_);
            });

            await Task.WhenAll(dbCreationTasks);

            await InitTestDatabase();
        }

        public void Dispose()
        {
            _client = null;
            _database = null;
        }

        private Task InitTestDatabase()
        {
            if (_appSettings.IsTestRun)
            {
                var initTasks = new Task[] {
                    InitCollection<BirdModel>(@"../data/data.birds.json", "birds"),
                    InitCollection<LocationModel>(@"../data/data.locations.json", "locations"),
                    InitCollection<PhotoModel>(@"../data/data.photos.json", "photos")
                };

                return Task.WhenAll(initTasks);
            }
            else return Task.CompletedTask;
        }

        private async Task InitCollection<T>(string fileName, string collectionName)
        {
            var collection = Database.GetCollection<T>(collectionName);
            if (await collection.AsQueryable().AnyAsync())
                return;

            var path = Path.Combine(_hostingEnvironment.ContentRootPath, fileName);
            var fileContent = await File.ReadAllTextAsync(path);
            var models = JsonConvert.DeserializeObject<List<T>>(fileContent);
            await collection.InsertManyAsync(models);
        }
    }
}