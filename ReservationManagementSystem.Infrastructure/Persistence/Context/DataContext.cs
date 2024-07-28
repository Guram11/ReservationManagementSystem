using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Infrastructure.Persistence.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Guest> Guests { get; set; }
}
