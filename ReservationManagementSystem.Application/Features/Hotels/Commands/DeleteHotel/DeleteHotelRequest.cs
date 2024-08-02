using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;

public sealed record DeleteHotelRequest(Guid Id) : IRequest<HotelResponse>;
