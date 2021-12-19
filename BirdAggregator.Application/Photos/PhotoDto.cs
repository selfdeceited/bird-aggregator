using System;
using System.Collections.Generic;

namespace BirdAggregator.Application.Photos
{
    public class PhotoDto
    {
        public string Src { get; set; }
        public string Caption { get; set; }
        public string Id { get; set; }
        public DateTime DateTaken { get; set; }
        public int Height { get; set; }
        public double Width { get; set; }
        public string Original { get; set; }
        public string Text { get; set; }
        public IEnumerable<string> BirdIds { get; set; }
        public string HostingLink { get; set; }
    }
}