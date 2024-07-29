using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Domain.Common;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Persistence.Context;
using ReservationManagementSystem.Infrastructure.FilterExtensions;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DataContext Context;

    public BaseRepository(DataContext context)
    {
        Context = context;
    }

    public void Create(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public Task<T?> Get(Guid id, CancellationToken cancellationToken)
    {
        return Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<List<T>> GetAll(CancellationToken cancellationToken, string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true,
        int pageNumber = 1, int pageSize = 10)
    {
        var filterExpression = FilterExtensions.FilterExtensions.GetFilterExpression<T>(filterOn, filterQuery);
        var sortExpression = FilterExtensions.FilterExtensions.GetSortExpression<T>(sortBy);

        return Context.Set<T>()
                .ApplyFilter(filterExpression)
                .ApplySort(sortExpression, isAscending)
                .ApplyPagination(pageNumber, pageSize)
                .ToListAsync(cancellationToken);
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
