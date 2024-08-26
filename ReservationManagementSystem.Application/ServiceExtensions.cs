using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;

namespace ReservationManagementSystem.Application;

public static class ServiceExtensions
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient<CheckAvailabilityHandler>();
    }
}