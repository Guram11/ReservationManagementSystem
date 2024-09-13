using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.FunctionalTests.Abstractions;

public class CustomWebApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<DataContext>));

            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("ReservationSystemDbTest");
            });

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        });
    }

    private static DataContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        return dbContext;
    }
}
