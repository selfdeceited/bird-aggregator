using System;
using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Domain.Birds
{
    public class Bird: IAggregateRoot
    {
        public Bird(int id, string name) {
            if (id <= 0) {
                throw new ArgumentException(nameof(id));
            }

            Id = id;
            Func<string, int> indexOf =  name.IndexOf;
            EnglishName = name.Substring(0, indexOf("(") - 1);
            LatinName = name.Substring(indexOf("(") + 1, indexOf(")") - indexOf("(") - 1);
        }
        
        public int Id { get; set; }
        public string LatinName { get; set; }
        public string EnglishName { get; set; }
    }
}
