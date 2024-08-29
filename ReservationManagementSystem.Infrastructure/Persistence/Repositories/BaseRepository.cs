using Microsoft.EntityFrameworkCore;
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

    public async Task<T> Create(T entity, CancellationToken cancellationToken)
    {
       Context.Set<T>().Add(entity);
       await Context.SaveChangesAsync(cancellationToken);

       return entity;
    }

    public Task<T?> Get(Guid id, CancellationToken cancellationToken)
    {
        return Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<List<T>> GetAll(string? filterOn, string? filterQuery,
        string? sortBy, bool isAscending,
        int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var filterExpression = FilterExtensions.FilterExtensions.GetFilterExpression<T>(filterOn, filterQuery);
        var sortExpression = FilterExtensions.FilterExtensions.GetSortExpression<T>(sortBy);

        return Context.Set<T>()
                .ApplyFilter(filterExpression)
                .ApplySort(sortExpression, isAscending)
                .ApplyPagination(pageNumber, pageSize)
                .ToListAsync(cancellationToken);
    }

    public async Task<T?> Update(Guid id, T entity, CancellationToken cancellationToken)
    {
        var existingEntity = await Context.Set<T>().FindAsync(id, cancellationToken);
        if (existingEntity == null)
        {
            return null;
        }

        Context.Entry(existingEntity).CurrentValues.SetValues(entity);
        Context.Entry(existingEntity).State = EntityState.Modified;
        await Context.SaveChangesAsync(cancellationToken);
        return existingEntity;
    }

    public async Task<T?> Delete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await Context.Set<T>().FindAsync(id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
        return entity;
    }
}
