using BirdAggregator.Host;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace BirdAggregator.Tests.Integration
{
    public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

        }
    }
}
