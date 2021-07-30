using System.Linq;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using BirdAggregator.Infrastructure.Flickr;

namespace BirdAggregator.Infrastructure.DataAccess.Mappings
{
    internal class PhotoMapper: DomainMapper<PhotoResultModel, Photo>
    {
        private readonly BirdMapper _birdMapper = new BirdMapper();
        private readonly LocationMapper _locationMapper = new LocationMapper();
        public override Photo ToDomain(PhotoResultModel model)
        {
            var birds = model.BirdModels.Select(_birdMapper.ToDomain);
            var location = _locationMapper.ToDomain(model.PhotoModel.Location);

            var flickrPhotoInformation = new FlickrPhotoInformation(
                model.PhotoModel.Flickr.FlickrId,
                model.PhotoModel.Flickr.FarmId,
                model.PhotoModel.Flickr.ServerId,
                model.PhotoModel.Flickr.Secret
            );

            return new Photo(
                model.PhotoModel.Id,
                location,
                flickrPhotoInformation,
                birds,
                model.PhotoModel.DateTaken,
                model.PhotoModel.Ratio,
                model.PhotoModel.Description
            );
        }
    }
}