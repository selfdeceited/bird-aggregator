using MediatR;

namespace BirdAggregator.Application
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}