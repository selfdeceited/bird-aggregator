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
					Console.WriteLine($"taken: {photo.photo.dates.taken} {photo.photo.description._content}");
				}
			}

			Console.ReadKey();
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

		public static T HandleExceptions<T>(IRestResponse<T> response)
		{
			if (response.ErrorException != null)
				throw response.ErrorException;

			return response.Data;
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
