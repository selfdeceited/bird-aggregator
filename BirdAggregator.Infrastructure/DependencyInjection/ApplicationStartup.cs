﻿using BirdAggregator.Application.Configuration;
using System;
using System.Threading;
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
using BirdAggregator.Infrastructure.HttpClients;

namespace BirdAggregator.Infrastructure.DependencyInjection;

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

        var bootstrapTask = serviceProvider.GetService<IMongoConnection>().BootstrapDb(CancellationToken.None);
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

        services.AddSingleton(appSettings);
        
        services.AddHttpClient(HttpClientNames.Flickr, c =>
        {
            c.BaseAddress = new Uri("https://api.flickr.com");
        });
        services.AddHttpClient(HttpClientNames.Wikipedia, c =>
        {
            c.BaseAddress = new Uri("https://en.wikipedia.org");
        });
        
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}
