using ReservationManagementSystem.Application.Interfaces.Services;

namespace ReservationManagementSystem.Infrastructure.Identity.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
