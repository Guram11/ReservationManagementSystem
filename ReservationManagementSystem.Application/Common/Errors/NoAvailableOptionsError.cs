using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Common.Errors;

public static class NoAvailableOptionsError
{
    public static Error NoAvailableOptions() => new Error(
    "NoAvailableOptions", $"No available options where found for specified dates!");
}
