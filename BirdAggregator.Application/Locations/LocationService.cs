using System.Linq;
using System.Threading.Tasks;
using BirdAggregator.Application.Locations.GetLocations;
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

        public async Task<MarkerDto> GetAsync(Location location)
        {
            var photos = await _photoRepository.GetByLocationAsync(location.Id);
            var birds = photos
                .SelectMany(x => x.Birds)
                .GroupBy(x => x.Id)
                .Select(x => x.First());
                
            return new MarkerDto
            {
                Id = location.Id,
                X = location.Longitude,
                Y = location.Latitude,
                Birds = birds.Select(x=> new BirdMarkerDto {
                    Id = x.Id,
                    Name = x.EnglishName
                }).ToArray(),
                FirstPhotoUrl = _pictureHostingService.GetOriginal(photos.FirstOrDefault()?.PhotoInformation)
            };
        }
    }
}