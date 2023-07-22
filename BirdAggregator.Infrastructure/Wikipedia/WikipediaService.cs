using System;
using System.Net.Http;
using System.Threading.Tasks;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.HttpClients;

namespace BirdAggregator.Infrastructure.Wikipedia
{
    public class WikipediaService : IInformationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WikipediaService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IBirdInfo> Get(string englishName)
        {
            var extract = await CallWikipediaExtract(englishName);
            if (extract.Contains("may refer to"))
            {
                extract = await CallWikipediaExtract(englishName + "_(bird)");
            }

            return new BirdInfo
            {
                Description = extract,
                ImageLink = await CallWikipediaImages(englishName)
            };
        }

        private Task<string> CallWikipediaExtract(string englishName)
        {
            return QueryWikipedia(englishName, "extracts");
        }

        private Task<string> CallWikipediaImages(string englishName)
        {
            return QueryWikipedia(englishName, "images");
        }

        private async Task<string> QueryWikipedia(string englishName, string propertyName)
        {
            var name = englishName.Replace(" ", "%20");
            var requestUrl = $"w/api.php?format=json&action=query&prop={propertyName}&titles={name}&redirects=true";
            return await CallWikipedia(requestUrl);
        }

        private async Task<string> CallWikipedia(string requestUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientNames.Wikipedia);
            httpClient.BaseAddress = new Uri("https://en.wikipedia.org");
            return await httpClient.GetStringAsync(requestUrl);
        }
    }
}
