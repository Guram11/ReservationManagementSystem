using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;

public sealed record UpdateHotelRequest(Guid Id, string Name) : IRequest<Result<HotelResponse>>;
