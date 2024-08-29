using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Common;

public static class HotelErrors
{
    public static Error NotFound(Guid id) => new Error(
        ErrorType.NotFoundError, $"Hotel with ID {id} was not found.");
}
