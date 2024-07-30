using ReservationManagementSystem.Infrastructure.Identity.Interfaces;

namespace ReservationManagementSystem.Infrastructure.Identity.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
