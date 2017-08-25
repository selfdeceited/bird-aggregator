using System.Collections.Generic;
using birds.Dtos;

namespace birds.Services
{
    public class GalleryService
    {
        private readonly ApiContext _context;
        public GalleryService(ApiContext context)
        {
            _context = context;
        }

        public string GetBirdName(Domain.Photo x)
        {
            var bird = _context.Birds.Find(x.BirdId);
            return bird == null ? string.Empty : bird.EnglishName;
        }

        public IEnumerable<PhotoDto> GetGallery(IEnumerable<Domain.Photo> photos){
            foreach (var photo in photos)
            {
                yield return new PhotoDto {
                        Thumbnail = GetThumbnailUrl(photo),
                        Src = GetImageUrl(photo),
                        Caption = GetBirdName(photo),
                        Id = photo.Id
                    };
            }
        }

        private string GetThumbnailUrl(Domain.Photo photo)
        {
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}_n.jpg";
        }

        private string GetImageUrl(Domain.Photo photo)
        {
            return $"https://farm{photo.FarmId}.staticflickr.com/{photo.ServerId}/{photo.FlickrId}_{photo.Secret}_b.jpg";
        }
    }
}