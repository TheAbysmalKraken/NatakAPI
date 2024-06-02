using Catan.Core.Models;
using MediatR;

namespace Catan.Core;

public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Result>
    where TQuery : IQuery
{
}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
