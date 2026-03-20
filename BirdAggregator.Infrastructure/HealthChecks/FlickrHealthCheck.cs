using BirdAggregator.Domain.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Infrastructure.HealthChecks;

public class FlickrHealthCheck(
    IPhotoRepository photoRepository,
    IPictureHostingService hostingService) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // todo: fix
        var firstPhoto = await photoRepository.GetById("1");
        var websiteLink = hostingService.GetWebsiteLink(firstPhoto.PhotoInformation);

        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(websiteLink, cancellationToken);

            return response.IsSuccessStatusCode
                ? new HealthCheckResult(HealthStatus.Healthy)
                : new HealthCheckResult(HealthStatus.Unhealthy);
        }
        catch (Exception)
        {
            return new HealthCheckResult(HealthStatus.Unhealthy);
        }
    }
}
