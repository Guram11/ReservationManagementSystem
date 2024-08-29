using ReservationManagementSystem.Application.Enums;

namespace ReservationManagementSystem.Application.Wrappers;

public sealed record Error(ErrorType ErrorType, string Description)
{
    public static readonly Error None = new(ErrorType.None, string.Empty);
}
