namespace birds.Domain
{
    public class Photo
    {
        public int Id { get; set; }
        public int BirdId {get; set; }
        public string FlickrId { get; set; }
        public Location Location { get; set; }
    }
}