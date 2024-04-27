using MediatR;
using Domain.Abstractions;

namespace Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}