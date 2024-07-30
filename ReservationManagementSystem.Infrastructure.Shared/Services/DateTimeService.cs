using ReservationManagementSystem.Application.Interfaces.Services;

namespace ReservationManagementSystem.Infrastructure.Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
