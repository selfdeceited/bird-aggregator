using System;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace BirdAggregator.Infrastructure.Wikipedia
{
    
    public class WikipediaService : IInformationService
    {
        public Task<IBirdInfo> Get(string englishName)
        {
            var extract = CallWikipediaExtract(englishName);
            if (extract.Contains("may refer to")){
                extract = CallWikipediaExtract(englishName + "_(bird)");
            }

            return Task.FromResult(new BirdInfo {
                Description = extract,
                ImageLink = CallWikipediaImages(englishName)
            } as IBirdInfo);
        }

        public string CallWikipediaExtract(string englishName)
        {
            return QueryWikipedia(englishName, "extracts");
        }

        public string CallWikipediaImages(string englishName)
        {
            return QueryWikipedia(englishName, "images");
        }

        private string QueryWikipedia(string englishName, string propertyName)
        {
            var name = englishName.Replace(" ", "%20");
            var requestUrl = $"w/api.php?format=json&action=query&prop={propertyName}&titles={name}&redirects=true";
            return CallWikipedia(requestUrl);
        }

        private string CallWikipedia(string requestUrl){
	        var client = new RestClient { BaseUrl = new Uri("https://en.wikipedia.org") };
            client.UseNewtonsoftJson();
	        var request = new RestRequest { Resource = requestUrl };
	        var response = client.Execute(request);
            return response.Content;
        }
    }
}

