﻿using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RateRoomTypeRepository : BaseRepository<RateRoomType>, IRateRoomTypeRepository
{
    private readonly DataContext _context;

    public RateRoomTypeRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public new async Task<RateRoomType> Create(RateRoomType rateRoomType)
    {
        var rate = await _context.Rates.FirstOrDefaultAsync(x => x.Id == rateRoomType.RateId);
        var roomType = await _context.RoomTypes.FirstOrDefaultAsync(x => x.Id == rateRoomType.RoomTypeId);

        if (rate is null || roomType is null )
        {
            return null!;
        }

        await _context.RateRoomTypes.AddAsync(rateRoomType);
        await _context.SaveChangesAsync();

        return rateRoomType;
    }

    public async Task<RateRoomType?> GetRateRoomTypeWithRateTimelines(Guid rateId, Guid roomTypeId)
    {
        var rateRoomType = await _context.RateRoomTypes
           .Include(rt => rt.RateTimelines)
           .FirstOrDefaultAsync(rt => rt.RateId == rateId && rt.RoomTypeId == roomTypeId);

        if (rateRoomType == null)
        {
            return null;
        }

        return rateRoomType;
    }

    public async Task<RateRoomType?> Delete(Guid rateId, Guid roomTypeId)
    {
        var rateRoomType = await _context.RateRoomTypes
                .FirstOrDefaultAsync(rt => rt.RateId == rateId && rt.RoomTypeId == roomTypeId);

        if (rateRoomType == null)
        {
            return null;
        }

        _context.RateRoomTypes.Remove(rateRoomType);
        await _context.SaveChangesAsync();

        return rateRoomType;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
