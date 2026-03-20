using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Locations;

public record GetLocationsQuery : IQuery<LocationListDto>;
public class GetLocationsQueryHandler(
    IPhotoRepository photoRepository,
    ILocationService locationService)
    : IQueryHandler<GetLocationsQuery, LocationListDto>
{
    public async Task<LocationListDto> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
    {
        var photos = await photoRepository.GetAllAsync();
        var markers = photos
            .Select(locationService.GetMarker)
            .Where(m => m != null);

        return new LocationListDto
        {
            Markers = [.. markers]
        };
    }
}
