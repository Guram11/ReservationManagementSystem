using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> Create(T entity, CancellationToken cancellationToken);
    Task<T?> Update(Guid id, T entity, CancellationToken cancellationToken);
    Task<T?> Delete(Guid id, CancellationToken cancellationToken);
    Task<T?> Get(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(string? filterOn, string? filterQuery,
        string? sortBy, bool isAscending,
        int pageNumber, int pageSize, CancellationToken cancellationToken);
}
