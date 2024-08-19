﻿using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.DeleteReservationRoomPayment;

public sealed record DeleteReservationRoomPaymentRequest(Guid Id) : IRequest<Result<ReservationRoomPaymentsResponse>>;
