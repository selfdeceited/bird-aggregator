using System.Collections.Generic;
using System.Linq;
using birds.Domain;

namespace birds.Dao
{
    public class BirdDao
    {
        // todo: fix EF in-memory DB hacks, it's disgusting!
        private readonly ApiContext _context;
        public BirdDao(ApiContext context)
        {
            _context = context;
        }
        public Bird Find(int id)
        {
            return _context.Birds.Find(id);
        }

        public IEnumerable<Bird> GetAll()
        {
            return _context.Birds.ToList();
        }

        public IEnumerable<Bird> GetBirds(Photo photo) 
        {
            return _context.Birds
                .Where(x => photo.BirdIds.Contains(x.Id));
        }

        public IEnumerable<Bird> GetBirdsByNames(IEnumerable<string> birdNames) 
        {
            return _context.Birds.Where(x => birdNames.Contains(x.ApiName));
        }
    }
}