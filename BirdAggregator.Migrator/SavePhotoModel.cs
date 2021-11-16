using System;
using BirdAggregator.Migrator.ResponseModels;

namespace BirdAggregator.Migrator
{
    public record SavePhotoModel(PhotoResponse.Photo photo, Location location, Sizes sizes);
}