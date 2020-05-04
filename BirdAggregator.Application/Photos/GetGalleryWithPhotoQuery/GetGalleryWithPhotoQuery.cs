namespace BirdAggregator.Application.Photos.GetGalleryWithPhotoQuery
{
    public class GetGalleryWithPhotoQuery : IQuery<GetGalleryWithPhotoDto>
    {
        public int PhotoId { get;}
        public GetGalleryWithPhotoQuery(int id)
        {
            PhotoId = id;
        }       
    }
}