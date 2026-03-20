using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations;

public record GetLocationsByIdQuery(string PhotoId) : IQuery<LocationListDto>;
public class GetLocationsByIdQueryHandler(
    IPhotoRepository photoRepository,
    ILocationService locationService)
    : IQueryHandler<GetLocationsByIdQuery, LocationListDto>
{
    public async Task<LocationListDto> Handle(GetLocationsByIdQuery request, CancellationToken cancellationToken)
    {
        var photo = await photoRepository.GetById(request.PhotoId);
        var mapMarker = locationService.GetMarker(photo);
        return new LocationListDto
        {
            Markers = mapMarker != null ? [
                mapMarker
            ] : []
        };
    }
}
