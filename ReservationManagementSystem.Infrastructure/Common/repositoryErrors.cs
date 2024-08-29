using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Infrastructure.Common;

public static class RepositoryErrors
{
    public static Error IsNull() => new Error(
        ErrorType.NotFoundError, $"Data is Null");
}
