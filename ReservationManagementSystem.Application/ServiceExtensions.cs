﻿using System.Reflection;
using ReservationManagementSystem.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace ReservationManagementSystem.Application;

public static class ServiceExtensions
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}