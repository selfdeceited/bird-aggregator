using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using birds.Domain;
using birds.Dtos;

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

        internal int GetPageCount()
        {
            return  _flickrConnectionService.GetPagesCount();
        }

        internal List<POCOs.PhotosResponse.Photo> GetMetaData(int count)
        {
            return Enumerable.Range(0, count)
                .SelectMany(page => _flickrConnectionService.GetPhotos(page).photos.photo)
                .Where(x => x.title.StartsWith("B: ")).ToList();
        }

        public void TruncateDb(){
            _context.Locations.RemoveRange(_context.Locations);
            _context.Photos.RemoveRange(_context.Photos);
            _context.Birds.RemoveRange(_context.Birds);
        }

        internal async Task<bool> SavePhotoAsync(POCOs.PhotosResponse.Photo photo)
        {
            if (_context.Photos.Any(x => x.FlickrId == photo.id))
                return false;

            var bird = _context.Birds.SingleOrDefault(_ => _.ApiName == photo.title);
            var extraPhotoInfo = _flickrConnectionService.GetPhoto(photo.id);
            var sizeInfo = _flickrConnectionService.GetSize(photo.id);

            var mediumSize = sizeInfo.sizes.size.Select(x => new SizeDto {
                    Width = x.width,
                    Height = x.height,
                    Label = x.label,
                    Source = x.source
                }).FirstOrDefault(x => x.Label == "Medium");
                
            var ratio = (double)mediumSize.Width / mediumSize.Height;
            
            var domainPhoto = new Photo
            {
                FlickrId = photo.id,
                FarmId = photo.farm,
                ServerId = photo.server,
                Secret = photo.secret,
                DateTaken = DateTime.Parse(extraPhotoInfo.photo.dates.taken),
                Description = extraPhotoInfo.photo.description._content,
                Ratio = ratio
            };

            if (bird != null)
                domainPhoto.BirdId = bird.Id;

            domainPhoto = PopulateLocation(domainPhoto);

            await _context.Photos.AddAsync(domainPhoto);
            return true;
        }

        internal bool AnythingSaved()
        {
            return _context.Photos.Any();
        }

        internal async Task<bool> SaveBirdAsync(string bird)
        {
            if (bird.Contains("undefined"))
                return false;

            var noPrefixName = bird.Replace("B: ", "");
            var engName = noPrefixName.Substring(0, noPrefixName.IndexOf("(") - 1);
            var latinName = noPrefixName.Substring(noPrefixName.IndexOf("(") + 1, noPrefixName.IndexOf(")") - noPrefixName.IndexOf("(") - 1);
            _context.Birds.Add(new Bird
            {
                ApiName = bird,
                EnglishName = engName,
                LatinName = latinName
            });
            await _context.SaveChangesAsync();
            return true;
        }


        private Photo PopulateLocation(Photo domainPhoto)
        {
            var locationResponse = _flickrConnectionService.GetLocation(domainPhoto.FlickrId);
            var location = locationResponse?.photo?.location;

            if (location == null)
                return domainPhoto;

            var domainLocation = _context.Locations.SingleOrDefault(_ =>
                _.Y == location.latitude && _.X == location.longitude);

            if (domainLocation == null)
            {
                var newLocation = new Location
                {
                    GeoTag = location.place_id,
                    Neighbourhood = location?.neighbourhood?._content,
                    Region = location?.region?._content,
                    Country = location?.country?._content,
                    X = location?.longitude,
                    Y = location?.latitude
                };

                _context.Locations.Add(newLocation);
                _context.SaveChanges();

                domainPhoto.LocationId = newLocation.Id;
            }
            else
                domainPhoto.LocationId = domainLocation.Id;

            return domainPhoto;
        }
    }
}