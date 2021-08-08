using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.LifeList.GetLifeListPerYearQuery
{
    public class GetLifeListPerYearQueryHandler : IQueryHandler<GetLifeListPerYearQuery, GetLifeListPerYearDto>
    {
        private readonly IPhotoRepository _photoRepository;
        public GetLifeListPerYearQueryHandler(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<GetLifeListPerYearDto> Handle(GetLifeListPerYearQuery request, CancellationToken cancellationToken)
        {
            var allPhotos = await _photoRepository.GetAllAsync();
            
            var yearsGrouping = allPhotos.GroupBy(x => x.DateTaken.Year);
                      
            return new GetLifeListPerYearDto
            {
                PerYearCollection = yearsGrouping.Select(x => new PerYearInfo
                {
                    Key = x.Key,
                    Count = x.SelectMany(_ => _.Birds).GroupBy(x => x.Id).Select(x => x.First()).Count()
                }).ToList()
            };
        }
    }
}