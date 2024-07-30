using ReservationManagementSystem.Infrastructure.Identity.Wrappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ReservationManagementSystem.Infrastructure.Identity.Contexts;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using System.Text;
using Infrastructure.Identity.Services;
using ReservationManagementSystem.Infrastructure.Identity.Interfaces;
using ReservationManagementSystem.Infrastructure.Identity.Services;

namespace ReservationManagementSystem.Infrastructure.Identity;

public static class ServiceExtensions
{
    public static void AddIdentityInfrastructure(this IServiceCollection services)
    {

        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(
                "Server=localhost;Database=ReservationManagementSystemAuthDb;Trusted_Connection=True;TrustServerCertificate=True",
                b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
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
                    ValidIssuer = "CoreIdentity",
                    ValidAudience = "CoreIdentityUser",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C1CF4B7DC4C4175B6618DE4F55CA4srtsrt"))
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
