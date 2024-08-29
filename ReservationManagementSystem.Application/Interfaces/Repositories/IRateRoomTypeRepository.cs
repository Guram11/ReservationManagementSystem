using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IRateRoomTypeRepository : IBaseRepository<RateRoomType>
{
    Task<RateRoomType?> Delete(Guid rateId, Guid roomTypeId, CancellationToken cancellationToken);
    Task<RateRoomType?> GetRateRoomTypeWithRateTimelines(Guid rateId, Guid roomTypeId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
