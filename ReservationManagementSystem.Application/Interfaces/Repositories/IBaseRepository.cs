using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> Create(T entity);
    Task<T?> Update(Guid id, T entity);
    Task<T?> Delete(Guid id);
    Task<T?> Get(Guid id);
    Task<List<T>> GetAll(string? filterOn, string? filterQuery,
        string? sortBy, bool isAscending,
        int pageNumber, int pageSize);
}
