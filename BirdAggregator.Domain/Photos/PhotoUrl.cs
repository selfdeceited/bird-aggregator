namespace BirdAggregator.Domain.Photos
{
    public class PhotoUrl
    {
        public PhotoUrl(FlickrPhoto flickr)
        {
            Original = GetFlickrImageUrl(flickr, "_h");
            Thumbnail = GetFlickrImageUrl(flickr, "_n");
        }

        public string Original { get; }
        public string Thumbnail { get;  }

        private string GetFlickrImageUrl(FlickrPhoto photo, string postfix){
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}{postfix}.jpg";
        }
    }
}