using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Infrastructure.Common;

public static class AuthErrors
{
    public static Error Unauthorized() => new Error(
        ErrorType.Unauthorized, "You are not Authorized!");

    public static Error Forbidden() => new Error(
        ErrorType.Forbidden, "You are not authorized to access this resource");
}
