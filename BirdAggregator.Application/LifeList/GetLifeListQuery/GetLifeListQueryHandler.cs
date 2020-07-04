using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;

namespace BirdAggregator.Application.LifeList.GetLifeListQuery
{
    public class GetLifeListQueryHandler : IQueryHandler<GetLifeListQuery, GetLifeListDto>
    {
        private readonly IBirdRepository _birdRepository;
        public GetLifeListQueryHandler(IBirdRepository birdRepository)
        {
            _birdRepository = birdRepository;
        }

        public async Task<GetLifeListDto> Handle(GetLifeListQuery request, CancellationToken cancellationToken)
        {
            /*
             var allPhotos = _context.Photos.ToList();
            var grouping = _birdDao
                .GetAll()
                .ToList()
                .Select(x => new LifeListGrouping
            {
                Bird = x,
                Photos = allPhotos.Where(p => p.BirdIds.Contains(x.Id))
            }).Where(x => x.Photos.Any());

            var localList = new List<LifeListDto>();
            foreach (var item in grouping)
            {
                var firstOccurence = item.Photos.Aggregate(
                    (c1, c2) => c1.DateTaken < c2.DateTaken ? c1 : c2);
                    
                localList.Add(new LifeListDto { 
                    BirdId = item.Bird.Id, 
                    Name = item.Bird.EnglishName, 
                    DateMet = firstOccurence.DateTaken,
                    Location = ShowLocation(firstOccurence.LocationId),
                    LocationId = firstOccurence.LocationId,
                    PhotoId = firstOccurence.Id,
                });
            }
            return localList.OrderByDescending(x => x.DateMet);
            */

            return new GetLifeListDto
            {
                FirstOccurences = new List<Occurence>() {
                    new Occurence {
                        BirdId = 42,
                        Name = "test-bird",
                        DateMet = DateTime.UtcNow,
                        Location = ShowLocation(42),
                        LocationId = 42,
                        PhotoId = 44,
                    }
                }
            };
        }

        private string ShowLocation(int locationId)
        {
            /*
            var location = _context.Locations.Find(locationId);
            if (location == null)
                return "unspecified location";

            Func<string, string> addComma = s => 
                string.IsNullOrEmpty(s) ? string.Empty : s + ",";

            return $"{addComma(location.Neighbourhood)} {addComma(location.Region)} {location.Country}";
            */
            return "unspecified location";
        }
    }
}