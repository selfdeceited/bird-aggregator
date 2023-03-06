namespace BirdAggregator.Application.Birds.GetBirdInfo
{
    public class GetBirdInfoQuery : IQuery<BirdInfoDto>
    {
        public string BirdId { get; }
        public GetBirdInfoQuery(string id)
        {
            BirdId = id;
        }
    }
}