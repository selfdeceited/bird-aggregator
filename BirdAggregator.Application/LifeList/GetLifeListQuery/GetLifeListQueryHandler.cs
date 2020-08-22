using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.LifeList.GetLifeListQuery
{
    public class GetLifeListQueryHandler : IQueryHandler<GetLifeListQuery, GetLifeListDto>
    {
        private readonly IBirdRepository _birdRepository;
        private readonly IPhotoRepository _photoRepository;

        public GetLifeListQueryHandler(IBirdRepository birdRepository, IPhotoRepository photoRepository)
        {
            _birdRepository = birdRepository;
            _photoRepository = photoRepository;
        }

        public async Task<GetLifeListDto> Handle(GetLifeListQuery request, CancellationToken cancellationToken)
        {
            // todo: move as much to domain logic as possible
            var allBirds = await _birdRepository.GetAll();
            var allPhotos = await _photoRepository.GetAllAsync();

            var grouping = allBirds.Select(x => new LifeListGrouping
            {
                Bird = x,
                Photos = allPhotos.Where(p => p.Birds.Select(_ => _.Id).Contains(x.Id))
            }).Where(x => x.Photos.Any());


            var localList = new List<Occurence>();
            foreach (var item in grouping)
            {
                var firstOccurence = item.Photos.Aggregate(
                    (c1, c2) => c1.DateTaken < c2.DateTaken ? c1 : c2);

                localList.Add(new Occurence
                {
                    BirdId = item.Bird.Id,
                    Name = item.Bird.EnglishName,
                    DateMet = firstOccurence.DateTaken,
                    Location = firstOccurence.Location.Description,
                    LocationId = firstOccurence.Location.Id,
                    PhotoId = firstOccurence.Id,
                });
            }

            return new GetLifeListDto
            {
                FirstOccurences = localList.OrderByDescending(x => x.DateMet).ToList()
            };
        }

        private class LifeListGrouping
        {
            public Bird Bird { get; set; }
            public IEnumerable<Photo> Photos { get; set; }
        }
    }
}