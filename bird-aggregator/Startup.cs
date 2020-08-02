using System.IO.Compression;
using bird_aggregator.Hubs;
using birds.Dao;
using birds.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace birds
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string CorsPolicy = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../bird-aggregator-client/build";
            });

            // todo: for dev env only!
            services.AddCors(options =>
            {
                var builder = new CorsPolicyBuilder()
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                options.AddPolicy(CorsPolicy, builder.Build());
            });

            services.AddSignalR();

            services.AddScoped<SeedHub>();
            
            services.Configure<GzipCompressionProviderOptions>(options => 
                options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression();

            var appSettings = Configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettings);
            services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("birds"));
           

            services.AddScoped<BirdDao>();
            services.AddScoped<FlickrConnectionService>();
            services.AddScoped<SeedService>();
            services.AddScoped<GalleryService>();
            services.AddScoped<WikipediaConnectionService>();

            services.AddScoped<SeedLauncher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(CorsPolicy);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseEndpoints(r => r.MapHub<SeedHub>("seed"));
            

            Console.WriteLine("env.ContentRootPath is " + env.ContentRootPath);
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = Path.Join(env.ContentRootPath, "..\\bird-aggregator-client");
                Console.WriteLine("spa.Options.SourcePath is " + spa.Options.SourcePath);

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
