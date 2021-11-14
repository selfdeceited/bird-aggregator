namespace BirdAggregator.Application.Photos.GetGalleryForBirdQuery
{
    public class GetGalleryForBirdQuery : IQuery<GetGalleryQueryDto>
    {
        public string BirdId { get; }
        public GetGalleryForBirdQuery(string id)
        {
            BirdId = id;
        }
    }
}