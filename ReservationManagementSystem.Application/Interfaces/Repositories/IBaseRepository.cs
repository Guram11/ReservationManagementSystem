﻿using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> Create(T entity);
    Task<T?> Update(Guid id, T entity);
    Task<T?> Delete(Guid id);
    Task<T?> Get(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(CancellationToken cancellationToken, string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true,
        int pageNumber = 1, int pageSize = 10);
}