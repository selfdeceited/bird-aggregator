using BirdAggregator.Application.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Infrastructure.Flickr;
using BirdAggregator.Infrastructure.Wikipedia;
using BirdAggregator.Application.Locations;
using BirdAggregator.Infrastructure.Mongo;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.DataAccess.Photos;

namespace BirdAggregator.Infrastructure.DependencyInjection
{
    public class ApplicationStartup
    {
        public static IServiceProvider Initialize(
            IServiceCollection services,
            AppSettings appSettings,
            InitializeOptions options)
        {
            var serviceProvider = CreateServiceProvider(services, appSettings);

            OnStartup(serviceProvider, options);
            return serviceProvider;
        }

        private static void OnStartup(IServiceProvider serviceProvider, InitializeOptions options)
        {
            if (!options.BootstrapDb)
                return;

            var bootstrapTask = serviceProvider.GetService<IMongoConnection>().BootstrapDb();
            bootstrapTask.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static IServiceProvider CreateServiceProvider(
            IServiceCollection services,
            AppSettings appSettings)
        {
            // todo: move to different extension methods
            services.AddScoped<IMongoConnection, MongoConnection>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<ILocationRepository, PhotoRepository>();
            services.AddScoped<IBirdRepository, PhotoRepository>();

            // todo: move to different extension methods
            services.AddScoped<IPictureHostingService, FlickrService>();
            services.AddScoped<IInformationService, WikipediaService>();

            // todo: move to different extension methods
            services.AddScoped<ILocationService, LocationService>();

            services.AddSingleton<AppSettings>(appSettings);
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}