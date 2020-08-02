namespace BirdAggregator.Application.Photos.GetGalleryForBirdQuery
{
    public class GetGalleryForBirdQuery : IQuery<GetGalleryQueryDto>
    {
        public int BirdId { get; }
        public GetGalleryForBirdQuery(int id)
        {
            BirdId = id;
        }
    }
}