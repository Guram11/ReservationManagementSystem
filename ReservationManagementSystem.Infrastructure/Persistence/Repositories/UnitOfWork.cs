using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Infrastructure.Persistence.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
    }
    public Task Save(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
