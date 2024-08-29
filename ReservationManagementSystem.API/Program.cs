using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application;
using Microsoft.AspNetCore.Identity;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using ReservationManagementSystem.Infrastructure.Identity.Seeds;
using ReservationManagementSystem.Infrastructure;
using ReservationManagementSystem.Infrastructure.Context;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(builder.Configuration["Logging:Address"]!, rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information()
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "ReservationmanagementSystemAPI", Version = "v1" });
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    },
                    Scheme = "Oauth2",
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header
                },
                new List<string>()
                }
            });
        });

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
        app.UseCors();
        app.MapControllers();
        app.Run();
    }
}