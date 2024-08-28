using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Infrastructure.Common;

public static class AuthErrors
{
    public static Error Unauthorized() => new Error(
        "Authorization.Unauthorized", "You are not Authorized!");

    public static Error Forbidden() => new Error(
        "Authorization.Forbidden", "You are not authorized to access this resource");
}
