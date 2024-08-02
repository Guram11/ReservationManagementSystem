using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;

public sealed record CreateHotelRequest(string Name) : IRequest<Result<HotelResponse>>;