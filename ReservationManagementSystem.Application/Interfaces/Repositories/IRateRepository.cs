﻿using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IRateRepository : IBaseRepository<Rate>
{
    Task<bool> IsRateInUseAsync(Guid id);
}
