using System.Collections.Generic;
using System.Linq;
using birds.Dao;
using birds.Domain;
using birds.Dtos;

namespace birds.Services
{
    public class GalleryService
    {
        private readonly BirdDao _birdDao;
        public GalleryService(BirdDao birdDao)
        {
            _birdDao = birdDao;
        }

        public IEnumerable<IdNameDto> GetBirdsForPhoto(Photo photo)
        {
            return _birdDao.GetBirds(photo).Select(x => new IdNameDto { Id = x.Id, Name = x.EnglishName });
        }

        public string GetBirdName(Photo photo)
        {
            return string.Join(", ", _birdDao.GetBirds(photo).Select(x => x.EnglishName));
        }

        public IEnumerable<PhotoDto> GetGallery(IEnumerable<Photo> photos)
        {
            return photos.Select(Project);
        }
        
        internal PhotoDto Project(Photo photo)
        {
            return new PhotoDto {
                        Original = GetImageUrl(photo),
                        Src = GetThumbnailUrl(photo),
                        Caption = GetBirdName(photo),
                        Id = photo.Id,
                        DateTaken = photo.DateTaken,
                        LocationId = photo.LocationId,
                        BirdIds = photo.BirdIds,
                        Height = 1,
                        Width = photo.Ratio,
                        Text = photo.Description
                    };
        }
        internal string GetPreviewUrl(Photo photo){
            return GetFlickrImageUrl(photo, "");
        }
        private string GetThumbnailUrl(Photo photo)
        {
            return GetFlickrImageUrl(photo, "_n");
        }

        private string GetImageUrl(Photo photo)
        {
            return GetFlickrImageUrl(photo, "_h");
        }

        // postfix info: https://www.flickr.com/services/api/misc.urls.html
        private string GetFlickrImageUrl(Photo photo, string postfix){
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}{postfix}.jpg";
        }
    }
}