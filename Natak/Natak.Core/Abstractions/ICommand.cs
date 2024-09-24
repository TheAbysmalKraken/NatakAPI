using Natak.Domain;
using MediatR;

namespace Natak.Core.Abstractions;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
