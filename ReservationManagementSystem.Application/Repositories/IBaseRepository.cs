using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Application.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    void Create(T entity);
    Task<T?> Update(Guid id, T entity);
    Task<T?> Delete(Guid id);
    Task<T?> Get(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(CancellationToken cancellationToken);
}
