﻿using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RoomTypeRepository : BaseRepository<RoomType>, IRoomTypeRepository
{
    private readonly DataContext _context;

    public RoomTypeRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<RoomType?> GetRoomTypeWithAvailabilityAsync(Guid roomTypeId, CancellationToken cancellationToken)
    {
        var roomType = await _context.RoomTypes
           .Include(rt => rt.AvailabilityTimelines)
           .FirstOrDefaultAsync(rt => rt.Id == roomTypeId, cancellationToken);

        if (roomType == null)
        {
            return null;
        }

        return roomType;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
