using System.Collections.Generic;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    internal class PhotoResultModel
    {
        public PhotoResultModel(PhotoModel photoModel, IEnumerable<BirdModel> birdModels, LocationModel locationModel)
        {
            PhotoModel = photoModel;
            BirdModels = birdModels;
            LocationModel = locationModel;
        }

        public PhotoModel PhotoModel { get; set; }
        public IEnumerable<BirdModel> BirdModels { get; set; }
        public LocationModel LocationModel { get; set; }
    }
}