using Catan.Domain;
using MediatR;

namespace Catan.Core.Abstractions;

public interface IQuery : IRequest<Result>;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
