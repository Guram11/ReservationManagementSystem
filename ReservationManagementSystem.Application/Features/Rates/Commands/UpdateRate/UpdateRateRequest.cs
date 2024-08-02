using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;

public sealed record UpdateRateRequest(Guid Id, string Name, Guid HotelID) : IRequest<RateResponse>;
