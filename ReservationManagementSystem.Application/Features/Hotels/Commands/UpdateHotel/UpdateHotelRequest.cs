using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;

public sealed record UpdateHotelRequest(Guid Id, string Name) : IRequest<HotelResponse>;
