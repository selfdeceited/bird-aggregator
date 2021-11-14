using System;
using BirdAggregator.Domain.Interfaces;

namespace BirdAggregator.Domain.Birds
{
    public class Bird: IAggregateRoot
    {
        public Bird(string id, string latinName, string englishName)
        {
            Id = id;
            LatinName = latinName;
            EnglishName = englishName;
        }

        public Bird(string id, string name)
        {
            Id = id;
            Func<string, int> indexOf =  name.IndexOf;
            EnglishName = name.Substring(0, indexOf("(") - 1);
            LatinName = name.Substring(indexOf("(") + 1, indexOf(")") - indexOf("(") - 1);
        }
        
        public string Id { get; set; }
        public string LatinName { get; set; }
        public string EnglishName { get; set; }
    }
}
