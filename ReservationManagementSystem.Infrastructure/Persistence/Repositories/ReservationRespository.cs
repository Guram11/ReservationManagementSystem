using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    private readonly DataContext _context;

    public ReservationRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsReservationInUseAsync(Guid id)
    {
        var isReservationInvoiceInUse = await _context.ReservationInvoices
           .AnyAsync(r => r.ReservationId == id);

        var isReservationRoomInUse = await _context.ReservationRooms
            .AnyAsync(r => r.ReservationId == id);


        return isReservationInvoiceInUse || isReservationRoomInUse;
    }
}
