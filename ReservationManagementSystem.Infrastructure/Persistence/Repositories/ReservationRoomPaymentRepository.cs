using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRoomPaymentRepository : BaseRepository<ReservationRoomPayments>, IReservationRoomPaymentRepository
{
    public ReservationRoomPaymentRepository(DataContext context) : base(context)
    {
    }
}
