using System.Collections.Generic;

namespace birds.POCOs
{
    public class SizeResponse: IStateResponse
    {
        public Sizes sizes { get; set; }
        public string stat { get; set; }
    }
    public class Size
    {
        public string label { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string media { get; set; }
    }

    public class Sizes
    {
        public int canblog { get; set; }
        public int canprint { get; set; }
        public int candownload { get; set; }
        public List<Size> size { get; set; }
    }
}