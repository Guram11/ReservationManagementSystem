﻿using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Domain.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Infrastructure.Persistence.FilterExtensions;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DataContext Context;

    public BaseRepository(DataContext context)
    {
        Context = context;
    }

    public async Task<T> Create(T entity)
    {
       Context.Set<T>().Add(entity);
       await Context.SaveChangesAsync();

       return entity;
    }

    public Task<T?> Get(Guid id)
    {
        return Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<T>> GetAll(string? filterOn, string? filterQuery,
        string? sortBy, bool isAscending,
        int pageNumber, int pageSize)
    {
        var filterExpression = FilterExtensions.FilterExtensions.GetFilterExpression<T>(filterOn, filterQuery);
        var sortExpression = FilterExtensions.FilterExtensions.GetSortExpression<T>(sortBy);

        return Context.Set<T>()
                .ApplyFilter(filterExpression)
                .ApplySort(sortExpression, isAscending)
                .ApplyPagination(pageNumber, pageSize)
                .ToListAsync();
    }

    public async Task<T?> Update(Guid id, T entity)
    {
        var existingEntity = await Context.Set<T>().FindAsync(id);
        if (existingEntity == null)
        {
            return null;
        }

        Context.Entry(existingEntity).CurrentValues.SetValues(entity);
        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
        return existingEntity;
    }

    public async Task<T?> Delete(Guid id)
    {
        var entity = await Context.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return null;
        }

        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
        return entity;
    }
}
