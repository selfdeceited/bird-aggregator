using System;
using System.Threading.Tasks;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;
using RestSharp;
using RestSharp.Serializers.Json;

namespace BirdAggregator.Infrastructure.Wikipedia
{
    public class WikipediaService : IInformationService
    {
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
            var client = new RestClient(new Uri("https://en.wikipedia.org"));
            client.UseSystemTextJson();
            var request = new RestRequest { Resource = requestUrl };
            var response = await client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
