using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using birds.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using bird_aggregator.Hubs;
using birds.Dao;

namespace birds
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSignalR();

            services.AddScoped<SeedHub>();
            
            services.Configure<GzipCompressionProviderOptions>(options => 
                options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression();

            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("birds"));
            
            var settings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(options => settings.Bind(options));
            
            services.AddScoped<BirdDao>();
            services.AddScoped<FlickrConnectionService>();
            services.AddScoped<SeedService>();
            services.AddScoped<GalleryService>();
            services.AddScoped<WikipediaConnectionService>();

            services.AddScoped<SeedLauncher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } 
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }
            
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes => routes.MapHub<SeedHub>("seed"));

            //new TaskFactory().StartNew(() =>
            //    app.ApplicationServices.GetService<SeedService>().Seed());
        }
    }
}
