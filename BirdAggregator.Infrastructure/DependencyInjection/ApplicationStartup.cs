using BirdAggregator.Application.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.Flickr;
using BirdAggregator.Infrastructure.DataAccess.Birds;
using BirdAggregator.Infrastructure.Wikipedia;

namespace BirdAggregator.Infrastructure.DependencyInjection
{
    public class ApplicationStartup
    {
        public static IServiceProvider Initialize(
            IServiceCollection services,
            AppSettings appSettings)
        {
            var serviceProvider = CreateServiceProvider(services, appSettings);

            return serviceProvider;
        }

        private static IServiceProvider CreateServiceProvider(
            IServiceCollection services,
            AppSettings appSettings)
        {                     
            services.AddScoped<IBirdRepository, BirdRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IPictureHostingService, FlickrService>();
            services.AddScoped<IInformationService, WikipediaService>();
            services.AddSingleton<AppSettings>(appSettings);
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}