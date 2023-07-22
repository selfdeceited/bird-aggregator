namespace BirdAggregator.Application.Configuration;

public class AppSettings
{
    public string FlickrUserId { get; set; } = null!;
    public string FlickrApiKey { get; set; } = null!;
    public string Github { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public bool IsTestRun { get; set; } = false;

    public string MongoConnectionString { get; set; } = null!;
}
