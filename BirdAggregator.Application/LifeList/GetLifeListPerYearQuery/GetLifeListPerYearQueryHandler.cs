using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Birds;

namespace BirdAggregator.Application.LifeList.GetLifeListPerYearQuery
{
    public class GetLifeListPerYearQueryHandler : IQueryHandler<GetLifeListPerYearQuery, GetLifeListPerYearDto>
    {
        private readonly IBirdRepository _birdRepository;
        public GetLifeListPerYearQueryHandler(IBirdRepository birdRepository)
        {
            _birdRepository = birdRepository;
        }

        public async Task<GetLifeListPerYearDto> Handle(GetLifeListPerYearQuery request, CancellationToken cancellationToken)
        {
            /*
                var yearsGrouping = _context.Photos.GroupBy(x => x.DateTaken.Year);
                return yearsGrouping.Select(x => new {x.Key, Count = x.SelectMany(_=>_.BirdIds).Distinct().Count()});
            */
            return new GetLifeListPerYearDto
            {
                PerYearCollection = new List<PerYearInfo>
                {
                    new PerYearInfo {
                        Key = 2020,
                        Count = 42
                    }
                }
            };
        }
    }
}