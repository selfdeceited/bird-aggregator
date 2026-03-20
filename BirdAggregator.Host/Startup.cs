using System.IO.Compression;
using BirdAggregator.Application.Birds.GetBirds;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Infrastructure.DependencyInjection;
using BirdAggregator.Infrastructure.HealthChecks;
using BirdAggregator.Infrastructure.Mongo;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.ResponseCompression;

namespace BirdAggregator.Host;

public class Startup(IWebHostEnvironment env)
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
            .AddJsonFile($"hosting.{env.EnvironmentName}.json")
            .AddEnvironmentVariables()
            .Build();
    private const string CorsPolicy = "_myAllowSpecificOrigins";


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
        });

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // todo: configure as env params!
        var allowedAddresses = new[] {
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

        services.Configure<GzipCompressionProviderOptions>(options =>
            options.Level = CompressionLevel.Optimal);

        services.AddResponseCompression();

        services.AddHttpLogging(_ => { });

        // todo: MediaTr IoC to infra layer!
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblyContaining<GetBirdsQuery>();
            c.RegisterServicesFromAssemblyContaining<MongoConnection>();
            c.RegisterServicesFromAssemblyContaining<Bird>();
        });

        var appSettings = _configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
        var sp = ApplicationStartup.Initialize(services, appSettings, new InitializeOptions());


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

        app.UseHttpLogging();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            endpoints.MapHealthChecks("/health");
        });


        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = string.Empty;
        });
    }
}
