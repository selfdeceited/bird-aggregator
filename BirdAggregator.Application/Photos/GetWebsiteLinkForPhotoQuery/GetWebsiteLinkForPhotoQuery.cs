namespace BirdAggregator.Application.Photos.GetWebsiteLinkForPhotoQuery
{
    public class GetWebsiteLinkForPhotoQuery : IQuery<GetWebsiteLinkForPhotoDto>
    {
        public int BirdId { get;}
        public GetWebsiteLinkForPhotoQuery(int id)
        {
            BirdId = id;
        }       
    }
}