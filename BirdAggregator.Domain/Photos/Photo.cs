using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Interfaces;
using BirdAggregator.Domain.Locations;

namespace BirdAggregator.Domain.Photos;

///<summary>
/// Photo root aggregate domain object
///</summary>
public class Photo(
    string id,
    Location location,
    IPhotoInformation photoInformation,
    IEnumerable<Bird> birds,
    DateTime dateTaken,
    double ratio,
    string description) : IAggregateRoot
{
    public string Id { get; } = id;
    public string Description { get; } = description;
    public double Ratio { get; } = ratio;
    public DateTime DateTaken { get; } = dateTaken;
    public Location Location { get; } = location;
    public IPhotoInformation PhotoInformation { get; } = photoInformation;
    public IEnumerable<Bird> Birds { get; } = birds;
    public string Caption => string.Join(", ", Birds.Select(x => x.EnglishName));
}
