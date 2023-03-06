using System;
using Colorify;
using Colorify.UI;
using BirdAggregator.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Migrator.Providers;
using BirdAggregator.Migrator.Repositories;
using BirdAggregator.Migrator.Services;

namespace BirdAggregator.Migrator
{
    public static class Program
    {
        public static readonly Format ColoredConsole = new(Theme.Dark);

        public static void Main(string[] args)
        {
            ColoredConsole.Clear();

            var serviceProvider = SetupDi();
            var migrator = serviceProvider.GetService<IMigrator>();

            try
            {
                migrator?.Run();
            }
            catch (Exception e)
            {
                ColoredConsole.WriteLine(e.Message, Colors.bgDanger);
            }
            finally
            {
                Console.ReadKey();
                ColoredConsole.ResetColor();
            }
        }

        private static IServiceProvider SetupDi()
        {
            var services = new ServiceCollection()
                .AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .AddSingleton<IPictureFetchingService, FlickrPhotoFetchingService>()
                .AddSingleton<IMigrationExecutor, MigrationExecutor>()
                .AddSingleton<IMigratorObservableProvider, MigratorObservableProvider>()
                .AddSingleton<IPhotoWriteRepository, PhotoWriteRepository>()
                .AddSingleton<IMigrator, Migrator>();

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json")
                .AddJsonFile($"appsettings.Development.json")
                //.AddJsonFile($"appsettings.{(args[0] == "-dev" ? "Development": "Production")}.json");
                .AddJsonFile($"hosting.json")
                .AddJsonFile($"hosting.Development.json");
            //.AddJsonFile($"hosting.{(firstArg == "-dev" ? "Development" : "Production")}.json");

            var configuration = builder.Build();
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            var serviceProvider = ApplicationStartup.Initialize(
                services, appSettings, new InitializeOptions { BootstrapDb = false });

            return serviceProvider;
        }
    }
}