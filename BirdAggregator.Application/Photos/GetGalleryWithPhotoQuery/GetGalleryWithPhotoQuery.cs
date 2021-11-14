namespace BirdAggregator.Application.Photos.GetGalleryWithPhotoQuery
{
    public class GetGalleryWithPhotoQuery : IQuery<GetGalleryWithPhotoDto>
    {
        public string PhotoId { get;}
        public GetGalleryWithPhotoQuery(string id)
        {
            PhotoId = id;
        }       
    }
}