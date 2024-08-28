using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.HotelServices.Common;

public static class HotelServiceErrors
{
    public static Error NotFound(Guid id) => new Error(
        "NotFound", $"Hotel service with ID {id} was not found.");
}
