using BirdAggregator.Domain.Photos;
using BirdAggregator.SharedKernel;

namespace BirdAggregator.Application.Locations;

public record GetLocationsByBirdIdQuery(string BirdId) : IQuery<LocationListDto>;
public class GetLocationsByBirdIdQueryHandler(
    ILocationService locationService,
    IPhotoRepository photoRepository) : IQueryHandler<GetLocationsByBirdIdQuery, LocationListDto>
{
    public async Task<LocationListDto> Handle(GetLocationsByBirdIdQuery request, CancellationToken cancellationToken)
    {
        var photos = await photoRepository.GetGalleryForBirdAsync(request.BirdId, SortDirection.Latest);
        var markers = photos
            .Select(locationService.GetMarker)
            .Where(m => m != null);

        return new LocationListDto
        {
            Markers = [.. markers]
        };
    }
}
