using birds.POCOs;
using System.Collections.Generic;
using System.Linq;
using birds.Domain;

namespace birds.Services
{
    public class SeedService
    {
        private readonly FlickrConnectionService _flickrConnectionService;
        private readonly ApiContext _context;
        public SeedService(FlickrConnectionService flickrConnectionService, ApiContext context){
            _flickrConnectionService = flickrConnectionService;
            _context = context;
        }

        public void TruncateDb(){
            _context.Locations.RemoveRange(_context.Locations);
            _context.Photos.RemoveRange(_context.Photos);
            _context.Birds.RemoveRange(_context.Birds);
        }
        public void Seed()
        {
            TruncateDb();
            var count = _flickrConnectionService.GetPagesCount();
            var allPhotos = Enumerable.Range(0, count)
                .SelectMany(page => _flickrConnectionService.GetPhotos(page).photos.photo)
                .Where(x => x.title.StartsWith("B: ")).ToList();

            var birdNames = allPhotos.GroupBy(x => x.title).Select(x => x.First().title);

            foreach(var bird in birdNames)
            {
                if (bird.Contains("undefined")) continue;
                var noPrefixName = bird.Replace("B: ", "");
                var engName = noPrefixName.Substring(0, noPrefixName.IndexOf("(") - 1);
                var latinName = noPrefixName.Substring(noPrefixName.IndexOf("(") + 1, noPrefixName.IndexOf(")") - noPrefixName.IndexOf("(") - 1);
                _context.Birds.Add(new Bird
                {
                    ApiName = bird,
                    EnglishName = engName,
                    LatinName = latinName,
                    Photos = new List<Domain.Photo>()
                });
                _context.SaveChanges();
            }

            foreach (var photo in allPhotos)
            {
                var bird = _context.Birds.SingleOrDefault(_ => _.ApiName == photo.title);
                var domainPhoto = new Domain.Photo
                {
                    FlickrId = photo.id
                };
                if (bird!=null)
                    domainPhoto.BirdId = bird.Id;
                    
                _context.Photos.Add(domainPhoto);
                _context.SaveChanges();
            }
        }
    }
}