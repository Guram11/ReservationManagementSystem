﻿using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;

public static class ReservationRoomPaymentErrors
{
    public static Error NotFound(Guid id) => new Error(
     "NotFound", $"ReservationRoomPayment with ID {id} was not found.");
}
