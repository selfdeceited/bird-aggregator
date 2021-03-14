using System.IO.Compression;
using System.Reflection;
using BirdAggregator.Application.Birds.GetBirds;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Infrastructure.DependencyInjection;
using BirdAggregator.Infrastructure.HealthChecks;
using BirdAggregator.Infrastructure.Mongo;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: UserSecretsId("c10e1a7d-8e00-44f5-a9ff-1a86af9e068a")]
namespace BirdAggregator.Host
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

            services.AddControllers();

            services.AddSwaggerGen();

            // todo: configure as env params!
            var allowedAddresses = new string[] {
                "http://localhost:3000",
                "http://localhost:10003",
            };

            services.AddCors(options =>
            {
                var builder = new CorsPolicyBuilder()
                    .WithOrigins(allowedAddresses)
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
                typeof(MongoConnection).Assembly,
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


            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

        }
    }
}
