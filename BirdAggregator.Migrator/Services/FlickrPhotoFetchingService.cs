using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application.Configuration;
using BirdAggregator.Infrastructure.HttpClients;
using BirdAggregator.Migrator.ResponseModels;
using Colorify;

namespace BirdAggregator.Migrator.Services
{
    public class FlickrPhotoFetchingService : IPictureFetchingService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public FlickrPhotoFetchingService(AppSettings appSettings, IHttpClientFactory httpClientFactory)
        {
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
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
            var request = CreateDefaultRequest("flickr.photos.getInfo", HttpMethod.Get,
            new Dictionary<string, string> { { "photo_id", hostingId } });

            var httpClient = _httpClientFactory.CreateClient(HttpClientNames.Flickr);
            var response = await httpClient.SendAsync(request, cancellationToken);
            return await HandleExceptions<PhotoResponse>(response);
        }

        public async Task<LocationResponse> GetLocation(string hostingId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientNames.Flickr);
            var request = CreateDefaultRequest("flickr.photos.geo.getLocation", HttpMethod.Get,
                new Dictionary<string, string> { { "photo_id", hostingId } });

            var response = await httpClient.SendAsync(request, cancellationToken);
            return await HandleExceptions<LocationResponse>(response);
        }

        public async Task<SizeResponse> GetSize(string hostingId, CancellationToken ct)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientNames.Flickr);
            var request = CreateDefaultRequest("flickr.photos.getSizes", HttpMethod.Get,
                new Dictionary<string, string> { { "photo_id", hostingId } });

            var response = await httpClient.SendAsync(request, ct);
            return await HandleExceptions<SizeResponse>(response);
        }

        private async Task<PhotosResponse> GetPhotos(CancellationToken cancellationToken, int page = 0)
        {
            var httpClient = _httpClientFactory.CreateClient(HttpClientNames.Flickr);
            var loadPerPage = _appSettings.IsTestRun ? 30 : 100;
            var request = CreateDefaultRequest("flickr.people.getPhotos", HttpMethod.Get,
            new Dictionary<string, string>
            {
                { "user_id", _appSettings.FlickrUserId },
                { "per_page", loadPerPage.ToString() },
                { "page", page > 0 ? page.ToString() : null },
            });

            var response = await httpClient.SendAsync(request, cancellationToken);
            return await HandleExceptions<PhotosResponse>(response);
        }

        private HttpRequestMessage CreateDefaultRequest(string method, HttpMethod httpMethod,
            Dictionary<string, string> onAddParams)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "api_key", _appSettings.FlickrApiKey },
                { "method", method },
                { "format", "json" },
                { "nojsoncallback", "1" }
            };

            var query = string.Join('&', queryParams
                .Concat(onAddParams)
                .Where(_ => _.Value != null)
                .Select(_ => $"{_.Key}={_.Value}"));

            return new HttpRequestMessage
            {
                RequestUri = new Uri($"services/rest?{query}", UriKind.Relative),
                Method = httpMethod,
                Headers = { CacheControl = new CacheControlHeaderValue { NoCache = true } }
            };
        }

        private static async Task<T> HandleExceptions<T>(HttpResponseMessage response) where T : class, IStateResponse
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = await response.Content.ReadAsStringAsync();
                Program.ColoredConsole.WriteLine(error, Colors.bgDanger);
                throw new HttpRequestException(error);
            }
            var content = await response.Content.ReadFromJsonAsync<T>();

            return content.stat == "ok" ? content : null;
        }
    }
}