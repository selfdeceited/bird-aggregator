using System;

namespace birds.Dtos
{
    public class PhotoDto
    {
        public string Src { get; set; }
        public string Caption { get; set; }
        public int Id { get; set; }
        public DateTime DateTaken { get; set; }
        public int LocationId { get; set; }
        public int BirdId { get; set; }
        public int Height { get; set; }
        public double Width { get; set; }
        public string Original { get; set; }
        public string Text { get; set; }
    }
}