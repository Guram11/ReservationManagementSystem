using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.DeleteRate;

public sealed record DeleteRateRequest(Guid Id) : IRequest<Result<RateResponse>>;
