using RestSharp;
using birds.POCOs;
using Microsoft.Extensions.Options;
using RestSharp.Serializers.NewtonsoftJson;

namespace birds.Services
{
    public class FlickrConnectionService
    {
        private readonly RestClient _client;
        private readonly AppSettings _settings;

        public FlickrConnectionService(IOptionsMonitor<AppSettings> settings){
            _settings = settings.CurrentValue;
            _client = new RestClient("https://api.flickr.com");
            _client.UseNewtonsoftJson();
        }

        public int GetPagesCount()
        {
	        return _settings.IsTestRun ? 5 : GetPhotos().photos.pages;
        }
        
        public PhotosResponse GetPhotos(int page = 0)
        {
            var loadPerPage = _settings.IsTestRun ? 30 : 100;
            var request = CreateDefaultRequest("flickr.people.getPhotos", Method.GET)
                .AddParameter("user_id", _settings.FlickrUserId)
                .AddParameter("per_page", loadPerPage);

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

        public LocationResponse GetLocation(string flickrPhotoId)
        {
            var request = CreateDefaultRequest("flickr.photos.geo.getLocation", Method.GET)
                .AddParameter("photo_id", flickrPhotoId);

            var response = _client.Execute<LocationResponse>(request);
            return HandleExceptions(response);
        }

        public SizeResponse GetSize(string flickrPhotoId)
        {
            var request = CreateDefaultRequest("flickr.photos.getSizes", Method.GET)
                .AddParameter("photo_id", flickrPhotoId);

            var response = _client.Execute<SizeResponse>(request);
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

            return response.Data.stat == "ok" ? response.Data : null;
        }
    }
}