using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations
{
    public record GetLocationsByIdQuery(string PhotoId) : IQuery<LocationListDto>;
    public class GetLocationsByIdQueryHandler : IQueryHandler<GetLocationsByIdQuery, LocationListDto>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly ILocationService _locationService;

        public GetLocationsByIdQueryHandler(IPhotoRepository photoRepository, ILocationService locationService)
        {
            _photoRepository = photoRepository;
            _locationService = locationService;
        }

        public async Task<LocationListDto> Handle(GetLocationsByIdQuery request, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.GetById(request.PhotoId);
            var mapMarker = _locationService.GetMarker(photo);
            return new LocationListDto
            {
                Markers = mapMarker != null ? new List<MarkerDto> {
                    mapMarker
                } : new List<MarkerDto>()
            };
        }
    }
}