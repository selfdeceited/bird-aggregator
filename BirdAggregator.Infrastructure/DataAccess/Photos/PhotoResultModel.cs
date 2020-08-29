using System.Collections.Generic;
using BirdAggregator.Infrastructure.DataAccess.Birds;
using BirdAggregator.Infrastructure.DataAccess.Locations;

namespace BirdAggregator.Infrastructure.DataAccess.Photos
{
    public partial class PhotoRepository
    {
        private class PhotoResultModel
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
}