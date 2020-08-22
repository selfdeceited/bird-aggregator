namespace BirdAggregator.Infrastructure.DataAccess.Birds
{
    internal class BirdModel
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
