using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationInvoiceRepository : BaseRepository<ReservationInvoices>, IReservationInvoiceRepository
{
    public ReservationInvoiceRepository(DataContext context) : base(context)
    {
    }
}
