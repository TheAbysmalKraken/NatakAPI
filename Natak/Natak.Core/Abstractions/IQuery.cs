using Natak.Domain;
using MediatR;

namespace Natak.Core.Abstractions;

public interface IQuery : IRequest<Result>;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
