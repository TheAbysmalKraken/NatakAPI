using Catan.Application.Models;
using MediatR;

namespace Catan.Core;

public interface IQuery : IRequest<Result>;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
