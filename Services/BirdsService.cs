using birds.POCOs;
using System.Collections.Generic;
using System.Linq;

namespace birds.Services
{
    public class BirdsService
    {
        private readonly FlickrConnectionService _flickrConnectionService;

        public BirdsService(FlickrConnectionService flickrConnectionService){
            _flickrConnectionService = flickrConnectionService;
        }

        public Dictionary<string, List<PhotosResponse.Photo>> GetBirds()
        {
            var count = _flickrConnectionService.GetPagesCount();
            var allPhotos = Enumerable.Range(0, count)
                .SelectMany(page => _flickrConnectionService.GetPhotos(page).photos.photo);

            var birdsScattered = allPhotos
                .Where(x => x.title.StartsWith("B: "))
                .GroupBy(x => x.id)
                .Select(x => x.First())
                .OrderBy(x => x.title)
                .ToList();

            var birdsGrouped = birdsScattered.GroupBy(x => x.title)
                .ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());
            
            return birdsGrouped;
        }
    }
}