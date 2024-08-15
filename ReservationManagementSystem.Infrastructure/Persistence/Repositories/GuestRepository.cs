using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class GuestRepository : BaseRepository<Guest>, IGuestRepository
{
    private readonly DataContext _context;

    public GuestRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Guest?> GetGuestByEmail(string email)
    {
        var guest  = await _context.Guests.FirstOrDefaultAsync(g => g.Email == email);

        if (guest == null)
        {
            return null;
        }

        return guest;
    }
}
