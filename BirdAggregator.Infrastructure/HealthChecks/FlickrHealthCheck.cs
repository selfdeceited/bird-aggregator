using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Infrastructure.HealthChecks
{
    public class FlickrHealthCheck : IHealthCheck
    {
        private IPhotoRepository _photoRepository;
        private IPictureHostingService _hostingService;

        public FlickrHealthCheck(IPhotoRepository photoRepository, IPictureHostingService hostingService)
        {
            _photoRepository = photoRepository;
            _hostingService = hostingService;
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var firstPhoto = await _photoRepository.GetById(1);
            var websiteLink = _hostingService.GetWebsiteLink(firstPhoto.PhotoInformation);

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(websiteLink);

                    if (response.IsSuccessStatusCode)
                    {
                        return new HealthCheckResult(HealthStatus.Healthy);
                    }
                    return new HealthCheckResult(HealthStatus.Unhealthy);
                }
            } catch (Exception) {
                return new HealthCheckResult(HealthStatus.Unhealthy);
            }
        }
    }
}
