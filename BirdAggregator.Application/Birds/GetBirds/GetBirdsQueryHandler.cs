using System.Threading;
using System.Threading.Tasks;
using BirdAggregator.Application.Configuration.Data;
using BirdAggregator.Application;
using BirdAggregator.Application.Birds.GetBirdsQuery;
using System.Collections.Generic;

namespace SampleProject.Application.Customers.GetCustomerDetails
{
    public class GetBirdsQueryHandler : IQueryHandler<GetBirdsQuery, BirdListDto>
    {
        public GetBirdsQueryHandler()
        {
            
        }

        public Task<BirdListDto> Handle(GetBirdsQuery request, CancellationToken cancellationToken)
        {
            const string query = ""; // TODO: LOGIC HERE
            //var connection = _connectionFactory.GetOpenConnection();

            //return connection.GetAsync<List<BirdDto>>(query, request);
            var fakeData = new BirdListDto {
                Birds = new List<BirdDto>{
                    new BirdDto {
                        Id = 1,
                        Name = "Bird 1",
                        Latin = "Avis 1"
                    },
                    new BirdDto {
                        Id = 2,
                        Name = "Bird 2",
                        Latin = "Avis 2"
                    }
                }
            };

            return Task.FromResult(fakeData);
        }
    }
}