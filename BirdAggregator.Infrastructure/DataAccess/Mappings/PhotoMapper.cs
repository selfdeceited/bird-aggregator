using System.Linq;
using BirdAggregator.Domain.Photos;
using BirdAggregator.Infrastructure.DataAccess.Photos;
using BirdAggregator.Infrastructure.Flickr;

namespace BirdAggregator.Infrastructure.DataAccess.Mappings
{
    internal class PhotoMapper : DomainMapper<PhotoResultModel, Photo>
    {
        private readonly BirdMapper _birdMapper = new();
        private readonly LocationMapper _locationMapper = new();
        public override Photo ToDomain(PhotoResultModel model)
        {
            var birds = model.BirdModels.Select(_birdMapper.ToDomain);
            var location = _locationMapper.ToDomain(model.Location);

            var flickrPhotoInformation = new FlickrPhotoInformation(
                model.Flickr.Id,
                model.Flickr.FarmId,
                model.Flickr.ServerId,
                model.Flickr.Secret
            );

            return new Photo(
                model.Id.ToString(),
                location,
                flickrPhotoInformation,
                birds,
                model.DateTaken,
                model.Ratio,
                model.Description
            );
        }
    }
}