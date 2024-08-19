﻿using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Commands.DeleteReservationRoomService;

public sealed record DeleteReservationRoomServiceRequest(Guid ReservationRoomId, Guid HotelServiceId) : IRequest<Result<ReservationRoomServiceResponse>>;
