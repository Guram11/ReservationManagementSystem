using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.Common;

public static class AvailibilityErrors
{
    public static Error NoAvailableOptions() => new Error(
        "NoAvailableOptions", $"No available options where found for specified dates!");

    public static Error ExceedingNumberOfRooms(int numberOfRooms) => new Error(
     "ExceedingNumberOfRooms", $"Requested available rooms exceed the total number of rooms ({numberOfRooms}).");
}
