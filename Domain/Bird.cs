using System.Collections.Generic;
 
namespace birds.Domain
{
    public class Bird
    {
        public int Id { get; set; }
        public string LatinName { get; set; }
        public string EnglishName { get; set; }
        public string ApiName { get; set; }
    }
}