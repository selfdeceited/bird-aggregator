using System;
using System.Collections.Generic;
using MediatR;

namespace BirdAggregator.Application.Birds.GetBirdInfo
{
    public class GetBirdInfoQuery : IQuery<BirdInfoDto>
    {
        public int BirdId { get; }
        public GetBirdInfoQuery(int id)
        {
            BirdId = id;
        }     
    }
}