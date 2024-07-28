using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Domain.Common;
using ReservationManagementSystem.Infrastructure.Persistence.Context;

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

    public Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return Context.Set<T>().ToListAsync(cancellationToken);
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
