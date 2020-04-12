using System;
using System.Collections.Generic;

namespace BirdAggregator.Application.Birds.GetBirdsQuery
{
    public class BirdListDto {
        public List<BirdDto> Birds { get;set; }
    }

    public class BirdDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Latin { get; set; }
    }
}