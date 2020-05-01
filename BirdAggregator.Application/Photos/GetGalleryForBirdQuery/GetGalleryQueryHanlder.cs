using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Domain.Photos;

namespace BirdAggregator.Application.Photos.GetGalleryForBirdQuery
{
    public class GetGalleryForBirdQueryHanlder : IQueryHandler<GetGalleryForBirdQuery, GetGalleryQueryDto>
    {
        private readonly IPhotoRepository _photoRepository;
        public GetGalleryForBirdQueryHanlder(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<GetGalleryQueryDto> Handle(GetGalleryForBirdQuery request, CancellationToken cancellationToken)
        {
            var gallery = await _photoRepository.GetGalleryForBirdAsync(request.BirdId);
            
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