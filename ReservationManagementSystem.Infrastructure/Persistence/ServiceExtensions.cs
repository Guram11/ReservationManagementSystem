using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Infrastructure.Persistence.Context;
using ReservationManagementSystem.Infrastructure.Persistence.Repositories;

namespace ReservationManagementSystem.Infrastructure.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options => options.UseSqlServer("Server=localhost;Database=ReservationManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGuestRepository, GuestRepository>();
    }
}