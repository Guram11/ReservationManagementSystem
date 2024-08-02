using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.DeleteRate;

public sealed record DeleteRateRequest(Guid Id) : IRequest<RateResponse>;
