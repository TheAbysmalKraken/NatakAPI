using Natak.Domain;
using MediatR;

namespace Natak.Core.Abstractions;

public interface IQueryHandler<in TQuery> : IRequestHandler<TQuery, Result>
    where TQuery : IQuery
{
}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
