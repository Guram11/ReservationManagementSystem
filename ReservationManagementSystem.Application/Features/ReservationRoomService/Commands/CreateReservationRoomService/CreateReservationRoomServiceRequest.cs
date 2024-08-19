using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Commands.CreateReservationRoomService;

public sealed record CreateReservationRoomServiceRequest(Guid ReservationRoomId, Guid HotelServiceId)
    : IRequest<Result<ReservationRoomServiceResponse>>;

