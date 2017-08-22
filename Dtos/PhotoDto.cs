namespace birds.Dtos
{
    public class PhotoDto
    {
        public string Src { get; set; }
        public string Thumbnail { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public int ThumbnailWidth { get {return 320;} }
    }
}