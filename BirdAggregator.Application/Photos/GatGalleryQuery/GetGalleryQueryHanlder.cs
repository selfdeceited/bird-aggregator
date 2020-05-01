using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Photos.GetGalleryQuery
{
    public class GetGalleryQueryHanlder: IQueryHandler<GetGalleryQuery, GetGalleryQueryDto>
    {
        private readonly IPhotoRepository _photoRepository;
        public GetGalleryQueryHanlder(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<GetGalleryQueryDto> Handle(GetGalleryQuery request, CancellationToken cancellationToken)
        {
            var gallery = await _photoRepository.GetAllAsync(request.Count);
            return new GetGalleryQueryDto {
                Photos = gallery.Select(_ => {
                   return new PhotoDto {
                        Original = _.Url.Original,
                        Src = _.Url.Thumbnail,
                        Caption = _.Caption,
                        Id = _.Id,
                        DateTaken = _.DateTaken,
                        LocationId = _.Location.Id,
                        BirdIds = _.Birds.Select(x => x.Id),
                        Height = 1,
                        Width = _.Ratio,
                        Text = _.Description
                    };
                }).ToList()
            };
        }
    }
}