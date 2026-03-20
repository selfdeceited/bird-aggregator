using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Domain.Locations;

public class Location(
    string country,
    string neighbourhood,
    string region,
    double x,
    double y,
    string locality) : IAggregateRoot
{
    private readonly Func<string, string> _addComma = s =>
                string.IsNullOrEmpty(s) ? string.Empty : s + ",";

    public string Description
        => !Specified
            ? "unspecified location"
            : $"{_addComma(Neighbourhood)} {_addComma(Region)} {Country}";

    public string Locality { get; } = locality;

    public bool Specified { get; } = true;
    public string Neighbourhood { get; } = neighbourhood;
    public string Region { get; } = region;
    public string Country { get; } = country;
    public double Latitude { get; } = y;
    public double Longitude { get; } = x;
}
