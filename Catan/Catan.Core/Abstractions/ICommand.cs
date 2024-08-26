using Catan.Domain;
using MediatR;

namespace Catan.Core.Abstractions;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
