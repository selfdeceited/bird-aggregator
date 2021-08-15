using System;
using Colorify;
using Colorify.UI;
using BirdAggregator.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using BirdAggregator.Application.Configuration;
using System.Text.Json;

namespace BirdAggregator.Migrator
{
    public class Program
    {
        public static Format ColoredConsole = new Format(Theme.Dark);

        public static void Main(string[] args)
        {
            ColoredConsole.Clear();

            var serviceProvider = SetupDi(args);
            var migrator = serviceProvider.GetService<IMigrator>();
            migrator.Run();

            ColoredConsole.WriteLine("Run completed. Press any key to close the app.");
            ColoredConsole.ResetColor();
            Console.ReadKey();
        }

        public static IServiceProvider SetupDi(string[] args)
        {
            var services = new ServiceCollection()
                .AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                 })
                
                .AddSingleton<IMigrator, Migrator>();

            var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json")
               .AddJsonFile($"appsettings.Development.json");
               //.AddJsonFile($"appsettings.{(args[0] == "-dev" ? "Development": "Production")}.json");

            var configuration = builder.Build();
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            ColoredConsole.WriteLine(JsonSerializer.Serialize(appSettings), Colorify.Colors.txtWarning);
            var serviceProvider = ApplicationStartup.Initialize(
                services, appSettings, new InitializeOptions { BootstrapDb = false });

            return serviceProvider;
        }
    }
}