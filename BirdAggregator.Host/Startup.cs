using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;
using Microsoft.AspNetCore.Cors.Infrastructure;
using BirdAggregator.Host.Configuration;
using MediatR;
using System.Reflection;
using BirdAggregator.Application.Birds.GetBirdsQuery;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Infrastructure.DataAccess.Birds;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Infrastructure.DependencyInjection;
using BirdAggregator.Infrastructure.HealthChecks;
using Microsoft.Extensions.Configuration.UserSecrets;

[assembly: UserSecretsId("c10e1a7d-8e00-44f5-a9ff-1a86af9e068a")]
namespace birds
{    
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly string CorsPolicy = "_myAllowSpecificOrigins";

        public Startup(IWebHostEnvironment env)
        {
            this._configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddJsonFile($"hosting.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .Build();
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

             /*services
                .AddHealthChecksUI()
                .AddInMemoryStorage();*/
            
            services.AddControllersWithViews();

            services.AddSwaggerDocumentation();

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

            //services.AddSignalR();

            services.Configure<GzipCompressionProviderOptions>(options => 
                options.Level = CompressionLevel.Optimal);

            services.AddResponseCompression();

            // todo: MediaTr IoC to infra layer!
            services.AddMediatR(Assembly.GetExecutingAssembly(),
                typeof(GetBirdsQuery).Assembly,
                typeof(BirdRepository).Assembly,
                typeof(Bird).Assembly
            );

            var appSettings = _configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            var sp = ApplicationStartup.Initialize(services, appSettings);

            services.AddHealthChecks()
                .AddCheck<FlickrHealthCheck>("flickr-access")
                .AddProcessAllocatedMemoryHealthCheck(200 * 1000 * 1000);
        }

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

                endpoints.MapHealthChecks("/health");
                //endpoints.MapHealthChecksUI();
            });

            //app.UseEndpoints(r => r.MapHub<SeedHub>("seed"));
            

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

            app.UseSwaggerDocumentation();
        }
    }
}