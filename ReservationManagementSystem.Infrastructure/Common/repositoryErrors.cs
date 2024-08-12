using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Infrastructure.Common;

public static class RepositoryErrors
{
    public static Error IsNull() => new Error(
        "RepositoryErrors.IsNull", $"Data is Null");
}
