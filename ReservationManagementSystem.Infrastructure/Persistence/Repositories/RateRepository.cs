using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;


namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class RateRepository : BaseRepository<Rate>, IRateRepository
{
    public RateRepository(DataContext context) : base(context)
    {
    }
}
