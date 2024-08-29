using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.HotelServices.Common;

public static class HotelServiceErrors
{
    public static Error NotFound(Guid id) => new Error(
        ErrorType.NotFoundError, $"Hotel service with ID {id} was not found.");
}
