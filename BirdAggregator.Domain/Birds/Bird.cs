using System;

namespace BirdAggregator.Domain.Birds
{
    public class Bird
    {
        public Bird(int id, string name) {
            if (id <= 0) {
                throw new ArgumentException(nameof(id));
            }

            Id = id;
            Func<string, int> _i =  name.IndexOf;
            EnglishName = name.Substring(0, _i("(") - 1);
            LatinName = name.Substring(_i("(") + 1, _i(")") - _i("(") - 1);
        }
        
        public int Id { get; set; }
        public string LatinName { get; set; }
        public string EnglishName { get; set; }
    }
}
