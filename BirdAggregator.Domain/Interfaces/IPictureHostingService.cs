using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Domain.Interfaces
{
    public interface IPictureHostingService
    {
        string GetOriginal(IPhotoInformation photoInformation);
        string GetThumbnail(IPhotoInformation photoInformation);
        string GetWebsiteLink(IPhotoInformation photoInformation);
        PictureInfo GetAllImageLinks(IPhotoInformation photoInformation);
        string GetUserLink();
    }
}