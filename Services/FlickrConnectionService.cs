using RestSharp;
using birds;
using birds.POCOs;
using Microsoft.Extensions.Options;

namespace birds.Services
{
    public class FlickrConnectionService
    {
        private readonly RestClient _client;
        private readonly AppSettings _settings;

        public FlickrConnectionService(IOptions<AppSettings> settings){
            _settings = settings.Value;
            _client = new RestClient("https://api.flickr.com");
        }

        public int GetPagesCount(){
            return GetPhotos().photos.pages;
        }
        
        public PhotosResponse GetPhotos(int page = 0)
        {
            var request = CreateDefaultRequest("flickr.people.getPhotos", Method.GET)
                .AddParameter("user_id", _settings.FlickrUserId);

            if (page > 0)
                request.AddParameter("page", page.ToString());

            var response = _client.Execute<PhotosResponse>(request);
            return HandleExceptions(response);
        }

        public PhotoResponse GetPhoto(string id)
        {
            var request = CreateDefaultRequest("flickr.photos.getInfo", Method.GET)
                .AddParameter("photo_id", id);

            var response = _client.Execute<PhotoResponse>(request);
            return HandleExceptions(response);
        }

        public LocationResponse GetLocation(RestClient client, string id)
        {
            var request = CreateDefaultRequest("flickr.photos.geo.getLocation", Method.GET)
                .AddParameter("photo_id", id);

            var response = client.Execute<LocationResponse>(request);
            return HandleExceptions(response);
        }

        public IRestRequest CreateDefaultRequest(string method, Method verb)
        {
            return new RestRequest("services/rest", verb)
                .AddParameter("method", method)
                .AddParameter("api_key", _settings.FlickrApiKey)
                .AddParameter("format", "json")
                .AddParameter("nojsoncallback", "1")
                .AddHeader("Cache-Control", "no-cache");
        }

        public T HandleExceptions<T>(IRestResponse<T> response) where T : class, IStateResponse
        {
            if (response.ErrorException != null)
                throw response.ErrorException;

            if (response.Data.stat == "ok")
                return response.Data;
            else return (T)null;
        }
    }
}