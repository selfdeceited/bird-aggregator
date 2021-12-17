using System.Linq;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Locations;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IBirdRepository _birdRepository;
        private readonly IPictureHostingService _pictureHostingService;

        public LocationService(ILocationRepository locationRepository, IPhotoRepository photoRepository, IBirdRepository birdRepository, IPictureHostingService pictureHostingService)
        {
            _locationRepository = locationRepository;
            _photoRepository = photoRepository;
            _birdRepository = birdRepository;
            _pictureHostingService = pictureHostingService;
        }


        public MarkerDto GetMarker(Photo photo)
        {
            var location = photo.Location;
            if (location == null) return null;
            var firstPhotoUrl = _pictureHostingService.GetOriginal(photo.PhotoInformation);
            return new MarkerDto
            {
                X = location.Longitude,
                Y = location.Latitude,
                Birds = photo.Birds.Select(x => new BirdMarkerDto {
                    Id = x.Id,
                    Name = x.EnglishName
                }).ToArray(),
                FirstPhotoUrl = firstPhotoUrl
            };
        }
    }
}