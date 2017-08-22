using System;
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
            PopulateBirds(birdNames);
            PopulatePhotos(allPhotos, false);
        }

        public void PopulatePhotos(List<POCOs.PhotosResponse.Photo> allPhotos, bool populateLocations = true)
        {
            foreach (var photo in allPhotos)
            {
                var bird = _context.Birds.SingleOrDefault(_ => _.ApiName == photo.title);
                var extraPhotoInfo = _flickrConnectionService.GetPhoto(photo.id);
                var domainPhoto = new Photo
                {
                    FlickrId = photo.id,
                    FarmId = photo.farm,
                    ServerId = photo.server,
                    Secret = photo.secret,
                    DateTaken = DateTime.Parse(extraPhotoInfo.photo.dates.taken),
                    Description = extraPhotoInfo.photo.description._content,
                };

                if (bird != null)
                    domainPhoto.BirdId = bird.Id;

                if (populateLocations)
                    domainPhoto = PopulateLocation(domainPhoto);
                
                _context.Photos.Add(domainPhoto);
                _context.SaveChanges();
            }
        }

        private Photo PopulateLocation(Photo domainPhoto)
        {
            var locationResponse = _flickrConnectionService.GetLocation(domainPhoto.FlickrId);
            var location = locationResponse?.photo?.location;
            if (location != null)
            {
                var domainLocation = _context.Locations.SingleOrDefault(x => x.GeoTag == location.place_id);
                if (domainLocation == null)
                    {
                        domainPhoto.Location = new Location
                        {
                            GeoTag = location.place_id,
                            Neighbourhood = location?.neighbourhood?._content,
                            Region = location?.region?._content,
                            Country = location?.country?._content
                        };
                    }
                    else
                        domainPhoto.Location = domainLocation;
            }
            return domainPhoto;
        }

        public void PopulateBirds(IEnumerable<string> birdNames)
        {
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
                    Photos = new List<Photo>()
                });
                _context.SaveChanges();
            }
        }
    }
}