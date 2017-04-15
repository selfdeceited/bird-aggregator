using System;
using System.Linq;
using RestSharp;

namespace birds
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.flickr.com");
            var pages = GetPhotos(client).photos.pages;

            var allPhotos = Enumerable.Range(0, pages)
                .SelectMany(page => GetPhotos(client, page).photos.photo);

            var birds = allPhotos
                .Where(x => x.title.StartsWith("B: "))
                .GroupBy(x => x.id)
                .Select(x => x.First())
                .OrderBy(x => x.title)
                .ToList();


            var grouped = birds.GroupBy(x => x.title).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());


            foreach (var group in grouped)
            {
                Console.WriteLine("\n" + group.Key);
                foreach (var image in group.Value)
                {
                    var photo = GetPhoto(client, image.id);
                    var location = GetLocation(client, image.id)?.photo?.location;
                    if (location != null)
                    {
                        var neighbourhood = location?.neighbourhood?._content;
                        if (neighbourhood == null)
                            neighbourhood = string.Empty;
                        else
                            neighbourhood = neighbourhood + ", ";

                        var region = location?.region?._content;
                        if (region == null)
                            region = string.Empty;
                        else
                            region = region + ", ";

                        var country = location?.country?._content;
                        if (country == null) country = string.Empty;
                        Console.WriteLine($"taken: {photo.photo.dates.taken} at {neighbourhood}{region}{country}");
                    }
                    else
                    {
                        Console.WriteLine($"taken: {photo.photo.dates.taken} at the unspecified or unavailable location");
                    }
                }
            }

            Console.ReadKey();
        }

        public static LocationResponse GetLocation(RestClient client, string id)
        {
            var request = CreateDefaultRequest("flickr.photos.geo.getLocation", Method.GET)
                .AddParameter("photo_id", id);

            var response = client.Execute<LocationResponse>(request);
            return HandleExceptions(response);
        }
        public static PhotosResponse GetPhotos(RestClient client, int page = 0)
        {
            var request = CreateDefaultRequest("flickr.people.getPhotos", Method.GET)
                .AddParameter("user_id", "106265895@N05");

            if (page > 0)
                request.AddParameter("page", page.ToString());

            var response = client.Execute<PhotosResponse>(request);
            return HandleExceptions(response);
        }

        public static PhotoResponse GetPhoto(RestClient client, string id)
        {
            var request = CreateDefaultRequest("flickr.photos.getInfo", Method.GET)
                .AddParameter("photo_id", id);

            var response = client.Execute<PhotoResponse>(request);
            return HandleExceptions(response);
        }

        public static T HandleExceptions<T>(IRestResponse<T> response) where T : class, IStateResponse
        {
            if (response.ErrorException != null)
                throw response.ErrorException;

            if (response.Data.stat == "ok")
                return response.Data;
            else return (T)null;
        }

        public static IRestRequest CreateDefaultRequest(string method, Method verb)
        {
            return new RestRequest("services/rest", verb)
                .AddParameter("method", method)
                .AddParameter("api_key", "0bc2f0f2743df78c0764103b16222110")
                .AddParameter("format", "json")
                .AddParameter("nojsoncallback", "1")
                .AddHeader("Cache-Control", "no-cache");
        }
    }
}
