using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations
{
    public class LocationService(
        IPictureHostingService pictureHostingService) : ILocationService
    {
        public MarkerDto GetMarker(Photo photo)
        {
            var location = photo.Location;
            if (location == null) return null;
            var firstPhotoUrl = pictureHostingService.GetOriginal(photo.PhotoInformation);
            return new MarkerDto
            {
                X = location.Longitude,
                Y = location.Latitude,
                Birds = [.. photo.Birds.Select(x => new BirdMarkerDto
                {
                    Id = x.Id,
                    Name = x.EnglishName
                })],
                FirstPhotoUrl = firstPhotoUrl
            };
        }
    }
}
