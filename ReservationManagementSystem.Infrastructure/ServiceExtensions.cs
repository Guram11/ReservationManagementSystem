using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Infrastructure.Context;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using ReservationManagementSystem.Infrastructure.Identity.Services;
using ReservationManagementSystem.Infrastructure.Persistence.Repositories;
using System.Text;

namespace ReservationManagementSystem.Infrastructure;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DataContextConnectionString"),
            b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));

        services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.AddScoped<IGuestRepository, GuestRepository>();
        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    },
                };
            });
    }
}