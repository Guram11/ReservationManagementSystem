﻿using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IGuestRepository : IBaseRepository<Guest>
{
    Task<Guest?> GetGuestByEmail(string email, CancellationToken cancellationToken);
}
