using System;
using System.Collections.Generic;
using MediatR;

namespace BirdAggregator.Application.Birds.GetBirdsQuery
{
    public class GetBirdsQuery : IQuery<BirdListDto>
    {
        public GetBirdsQuery()
        {
            
        }
    }
}