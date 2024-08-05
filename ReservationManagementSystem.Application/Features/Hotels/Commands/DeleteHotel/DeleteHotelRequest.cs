using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;

public sealed record DeleteHotelRequest(Guid Id) : IRequest<Result<HotelResponse>>;
