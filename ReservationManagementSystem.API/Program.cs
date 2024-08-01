using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application;
using Microsoft.AspNetCore.Identity;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using ReservationManagementSystem.Infrastructure.Identity.Seeds;
using ReservationManagementSystem.Infrastructure;
using ReservationManagementSystem.Infrastructure.Context;
using Serilog;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information()
            .CreateLogger();

        builder.Logging.ClearProviders();

        builder.Logging.AddSerilog(logger);

        builder.Services.ConfigureApplication();
        builder.Services.ConfigurePersistence(builder.Configuration);

        builder.Services.ConfigureApiBehavior();
        builder.Services.ConfigureCorsPolicy();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        var serviceScope = app.Services.CreateScope();
        var dataContext = serviceScope.ServiceProvider.GetService<DataContext>();
        dataContext?.Database.EnsureCreated();

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