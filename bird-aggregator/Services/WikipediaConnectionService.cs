using System;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace birds.Services
{
    public class WikipediaConnectionService
    {
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