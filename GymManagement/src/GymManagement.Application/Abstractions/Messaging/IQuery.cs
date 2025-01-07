using GymManagement.Domain.Abstractions;
using MediatR;

namespace GymManagement.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;