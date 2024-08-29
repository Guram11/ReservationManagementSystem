using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Common;

public static class GuestErrors
{
    public static Error EmailAlreadyInUse() => new Error(
        ErrorType.AlreadyCreatedError, "Given Email is already in use!");

    public static Error NotFound(Guid id) => new Error(
        ErrorType.NotFoundError, $"Guest with ID {id} was not found.");
}
