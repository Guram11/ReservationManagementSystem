namespace ReservationManagementSystem.Infrastructure.Identity.Interfaces;

public interface IDateTimeService
{
    DateTime NowUtc { get; }
}
