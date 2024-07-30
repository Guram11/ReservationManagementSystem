using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Infrastructure.Persistence.Context;
using ReservationManagementSystem.Application;
using ReservationManagementSystem.Infrastructure.Persistence;
using ReservationManagementSystem.Infrastructure.Identity;
using ReservationManagementSystem.Infrastructure.Identity.Contexts;
using Microsoft.AspNetCore.Identity;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using ReservationManagementSystem.Infrastructure.Identity.Seeds;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigurePersistence();
        builder.Services.ConfigureApplication();
        builder.Services.AddIdentityInfrastructure();

        builder.Services.ConfigureApiBehavior();
        builder.Services.ConfigureCorsPolicy();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        var serviceScope = app.Services.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
        var authDataContext = serviceScope.ServiceProvider.GetService<IdentityContext>();
        dataContext?.Database.EnsureCreated();
        authDataContext?.Database.EnsureCreated();

        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await DefaultRoles.SeedAsync(roleManager);
        await DefaultSuperAdmin.SeedAsync(userManager);
        await DefaultBasicUser.SeedAsync(userManager);

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseErrorHandler();
        app.UseCors();
        app.MapControllers();
        app.Run();
    }
}