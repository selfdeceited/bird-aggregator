namespace BirdAggregator.Application.Photos.GetGalleryQuery
{
    public class GetGalleryQuery : IQuery<GetGalleryQueryDto>
    {
        public int Count { get;}
        public GetGalleryQuery(int count)
        {
            Count = count;
        }       
    }
}