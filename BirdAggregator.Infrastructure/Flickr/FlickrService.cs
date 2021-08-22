using BirdAggregator.Application.Configuration;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Infrastructure.Flickr
{
    public class FlickrService : IPictureHostingService
    {
        private readonly AppSettings _appSettings;

        public FlickrService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public PictureInfo GetAllImageLinks(IPhotoInformation photoInformation)
        {
            return new()
            {
                OriginalLink = GetOriginal(photoInformation),
                ThumbnailLink = GetThumbnail(photoInformation),
                WebsiteLink = GetWebsiteLink(photoInformation)
            };
        }

        public string GetOriginal(IPhotoInformation photoInformation) => GetFlickrImageUrl(photoInformation, "_h");
        public string GetThumbnail(IPhotoInformation photoInformation) => GetFlickrImageUrl(photoInformation, "_n");

        public string GetUserLink()
        {
            return $"https://www.flickr.com/photos/{_appSettings.FlickrUserId}/";
        }

        public string GetWebsiteLink(IPhotoInformation photoInformation)
        {
            return $"https://www.flickr.com/photos/{_appSettings.FlickrUserId}/{photoInformation.Id}";
        }

        private string GetFlickrImageUrl(IPhotoInformation photoInformation, string postfix)
        {
            if (photoInformation == null)
                return string.Empty;
                
            var farmId = photoInformation.Metadata["FarmId"];
            var serverId = photoInformation.Metadata["ServerId"];
            var secret = photoInformation.Metadata["Secret"];

            return $"https://farm{farmId}.staticflickr.com/{serverId}/{photoInformation.Id}_{secret}{postfix}.jpg";
        }
    }
}
