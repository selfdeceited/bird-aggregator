using System;
using Xunit;
using birds;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;

namespace BirdAggregator.Tests.Integration
{
    public class BasicTests : IClassFixture<TestingWebAppFactory<Startup>>
    {
        private readonly TestingWebAppFactory<Startup> _factory;
        private readonly WebApplicationFactoryClientOptions _options;

        public BasicTests(TestingWebAppFactory<Startup> factory)
        {
            _factory = factory;
            _options = new WebApplicationFactoryClientOptions {
                BaseAddress = new Uri("https://localhost:5001"),
                AllowAutoRedirect = true
            };
        }

        [Theory]
        [InlineData("/api/birds")]
        [InlineData("/api/gallery/bird/3")]
        [InlineData("/api/gallery/photo/1")]
        [InlineData("/api/gallery/50")]
        [InlineData("/api/gallery/photo/1/websitelink")]
        [InlineData("/api/birds/info/1")]
        [InlineData("/api/lifelist")]
        [InlineData("/api/lifelist/peryear")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient(_options);

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}
