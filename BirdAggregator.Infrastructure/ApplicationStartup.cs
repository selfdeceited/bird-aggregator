using BirdAggregator.Application.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace BirdAggregator.Infrastructure
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
            //services.AddScoped<SeedHub>();
            
            //services.Configure<AppSettings>(appSettings);
            //services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("birds"));
           
            /*services.AddScoped<BirdDao>();
            services.AddScoped<FlickrConnectionService>();
            services.AddScoped<SeedService>();
            services.AddScoped<GalleryService>();
            services.AddScoped<WikipediaConnectionService>();

            services.AddScoped<SeedLauncher>();

            var serviceProvider = services.*/
            throw new NotImplementedException();

            //return serviceProvider;
        }
    }
}