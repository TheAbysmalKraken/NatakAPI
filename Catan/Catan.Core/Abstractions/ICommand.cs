using Catan.Application.Models;
using MediatR;

namespace Catan.Core;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
