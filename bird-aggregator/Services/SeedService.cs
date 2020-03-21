using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using birds.Domain;
using birds.Dtos;
using birds.Dao;

namespace birds.Services
{
    public class SeedService
    {
        private readonly FlickrConnectionService _flickrConnectionService;
        private readonly ApiContext _context;
        private readonly BirdDao _birdDao;

        public SeedService(FlickrConnectionService flickrConnectionService, ApiContext context, BirdDao birdDao){
            _flickrConnectionService = flickrConnectionService;
            _context = context;
            _birdDao = birdDao;
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

        public void TruncateDb() {
            _context.Locations.RemoveRange(_context.Locations);
            _context.Photos.RemoveRange(_context.Photos);
            _context.Birds.RemoveRange(_context.Birds);
        }

        internal async Task<bool> SavePhotoAsync(POCOs.PhotosResponse.Photo photo)
        {
            if (_context.Photos.Any(x => x.FlickrId == photo.id))
                return false;

            var birds = _birdDao.GetBirdsByNames(ExtractBirdNames(photo.title));
            var extraPhotoInfo = _flickrConnectionService.GetPhoto(photo.id);
            var sizeInfo = _flickrConnectionService.GetSize(photo.id);

            var mediumSize = sizeInfo.sizes.size.Select(x => new SizeDto {
                    Width = x.width,
                    Height = x.height,
                    Label = x.label,
                    Source = x.source
                }).FirstOrDefault(x => x.Label == "Medium");
                
            var ratio = mediumSize != null ? (double)mediumSize.Width / mediumSize.Height : 1;

	        var domainPhoto = new Photo
	        {
		        FlickrId = photo.id,
		        FarmId = photo.farm,
		        ServerId = photo.server,
		        Secret = extraPhotoInfo.photo.originalsecret,
		        DateTaken = DateTime.Parse(extraPhotoInfo.photo.dates.taken),
		        Description = extraPhotoInfo.photo.description._content,
		        Ratio = ratio,
		        BirdIdsAsString = birds.Any() ? string.Join(",", birds.Select(x => x.Id)) : ""
	        };

	        domainPhoto = PopulateLocation(domainPhoto);

	        if (_context.Photos.FirstOrDefault(x => x.FlickrId == domainPhoto.FlickrId) != null)
		        return true;

	        await _context.Photos.AddAsync(domainPhoto);
	        await _context.SaveChangesAsync();

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

            Func<string, int> _i =  bird.IndexOf; // for readability in next lines
            var engName = bird.Substring(0, _i("(") - 1);
            var latinName = bird.Substring(_i("(") + 1, _i(")") - _i("(") - 1);
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

        public IEnumerable<string> ExtractBirdNames(IEnumerable<string> photoNames)
        {
            var birdNames = new HashSet<string>();

            Action<string> add = name => {
                if (!birdNames.Contains(name))
                    birdNames.Add(name);
            };

            foreach (var title in photoNames) {
                if (title.Contains(", ")) {
                    title.Split(", ").ToList().ForEach(add);
                } else {
                    add(title);
                }
            }
            return birdNames;
        }

        private IEnumerable<string> ExtractBirdNames(string title)
        {
            if (title.StartsWith("B: "))
                title = title.Substring("B: ".Length);
            return title.Contains(", ") ? title.Split(", ") : new[]{title};
        }
    }
}