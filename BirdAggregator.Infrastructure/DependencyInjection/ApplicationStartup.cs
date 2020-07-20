﻿using BirdAggregator.Application.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.Flickr;
using BirdAggregator.Infrastructure.DataAccess.Birds;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using BirdAggregator.Infrastructure.Wikipedia;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Infrastructure.DataAccess.Locations;

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
            // todo: move to different extension methods                  
            services.AddScoped<IBirdRepository, BirdRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();

            // todo: move to different extension methods                  
            services.AddScoped<IPictureHostingService, FlickrService>();
            services.AddScoped<IInformationService, WikipediaService>();
            
            services.AddSingleton<AppSettings>(appSettings);
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}