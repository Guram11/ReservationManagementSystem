using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;

public sealed record UpdateRateRequest(Guid Id, string Name, Guid HotelID) : IRequest<Result<RateResponse>>;
