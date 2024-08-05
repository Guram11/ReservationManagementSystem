using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.CreateRate;

public sealed record CreateRateRequest(string Name, Guid HotelId) : IRequest<Result<RateResponse>>;
