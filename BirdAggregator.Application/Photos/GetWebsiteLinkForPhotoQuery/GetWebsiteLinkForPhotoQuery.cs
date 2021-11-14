namespace BirdAggregator.Application.Photos.GetWebsiteLinkForPhotoQuery
{
    public class GetWebsiteLinkForPhotoQuery : IQuery<GetWebsiteLinkForPhotoDto>
    {
        public string BirdId { get;}
        public GetWebsiteLinkForPhotoQuery(string id)
        {
            BirdId = id;
        }       
    }
}