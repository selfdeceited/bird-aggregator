using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Migrator.ResponseModels;
using RestSharp;
using RestSharp.Serializers.Json;

namespace BirdAggregator.Migrator.Services
{
    public class FlickrPhotoFetchingService: IPictureFetchingService
    {
        private readonly RestClient _client;
        private readonly AppSettings _appSettings;
        
        public FlickrPhotoFetchingService(AppSettings appSettings) {
            _appSettings = appSettings;
            Console.WriteLine(JsonSerializer.Serialize(_appSettings));
            _client = new RestClient("https://api.flickr.com");
            _client.UseSystemTextJson();
        }
        
        public async Task<int> GetPages(CancellationToken cancellationToken)
        {
            var photosResponse = await GetPhotos(cancellationToken);
            return photosResponse.photos.pages;
        }

        public async Task<PhotoId[]> GetPhotoInfoForPage(int pageNumber, CancellationToken cancellationToken)
        {
            var photosResponse = await GetPhotos(cancellationToken, pageNumber);
            return photosResponse.photos.photo
                .Where(x => x.title.StartsWith("B: "))
                .Select(_ => new PhotoId(_.id, _.title))
                .ToArray();
        }
        
        public async Task<PhotoResponse> GetPhotoInfo(string hostingId, CancellationToken cancellationToken)
        {
            var request = CreateDefaultRequest("flickr.photos.getInfo", Method.Get)
                .AddParameter("photo_id", hostingId);

            var response = await _client.ExecuteAsync<PhotoResponse>(request, cancellationToken);
            return HandleExceptions(response);
        }

        public async Task<LocationResponse> GetLocation(string hostingId, CancellationToken cancellationToken)
        {
            var request = CreateDefaultRequest("flickr.photos.geo.getLocation", Method.Get)
                .AddParameter("photo_id", hostingId);

            var response = await _client.ExecuteAsync<LocationResponse>(request, cancellationToken);
            return HandleExceptions(response);
        }

        public async Task<SizeResponse> GetSize(string hostingId, CancellationToken ct)
        {
            var request = CreateDefaultRequest("flickr.photos.getSizes", Method.Get)
                .AddParameter("photo_id", hostingId);

            var response = await _client.ExecuteAsync<SizeResponse>(request, ct);
            return HandleExceptions(response);
        }

        private async Task<PhotosResponse> GetPhotos(CancellationToken cancellationToken, int page = 0)
        {
            var loadPerPage = _appSettings.IsTestRun ? 30 : 100;
            var request = CreateDefaultRequest("flickr.people.getPhotos", Method.Get)
                .AddParameter("user_id", _appSettings.FlickrUserId)
                .AddParameter("per_page", loadPerPage);

            if (page > 0)
                request.AddParameter("page", page.ToString());
            var response = await _client.ExecuteAsync<PhotosResponse>(request, cancellationToken);
            return HandleExceptions(response);
        }

        private RestRequest CreateDefaultRequest(string method, Method verb)
        {
            return new RestRequest("services/rest", verb)
                .AddParameter("method", method)
                .AddParameter("api_key", _appSettings.FlickrApiKey)
                .AddParameter("format", "json")
                .AddParameter("nojsoncallback", "1")
                .AddHeader("Cache-Control", "no-cache");
        }

        private T HandleExceptions<T>(RestResponse<T> response) where T : class, IStateResponse
        {
            if (response.ErrorException == null)
                return response.Data?.stat == "ok" ? response.Data : null;
            
            Program.ColoredConsole.WriteLine(response.ErrorException.Message, Colorify.Colors.bgDanger);
            throw response.ErrorException;
        }
    }
}