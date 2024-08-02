using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.CreateRate;

public sealed record CreateRateRequest(string Name, Guid HotelId) : IRequest<RateResponse>;
