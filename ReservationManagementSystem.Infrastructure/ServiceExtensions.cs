using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Infrastructure.Common;
using ReservationManagementSystem.Infrastructure.Context;
using ReservationManagementSystem.Infrastructure.Identity.Models;
using ReservationManagementSystem.Infrastructure.Identity.Services;
using ReservationManagementSystem.Infrastructure.Persistence.Repositories;
using ReservationManagementSystem.Infrastructure.Services.CurrencyRatesRetriever;
using ReservationManagementSystem.Infrastructure.Services.Email;
using System.Text;

namespace ReservationManagementSystem.Infrastructure;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DataContextConnectionString"),
            b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));

        // Polly Configuration
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
        services.AddHostedService<CurrencyRatesService>().AddHttpClient<ICurrencyRatesRetriever, CurrencyRatesService>()
            .AddPolicyHandler(retryPolicy);

        services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.Configure<CurrencyRatesSettings>(configuration.GetSection("CurrencyRatesSettings"));

        services.AddScoped<IGuestRepository, GuestRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IHotelServiceRepository, HotelServiceRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReservationInvoiceRepository, ReservationInvoiceRepository>();
        services.AddScoped<IReservationRoomRepository, ReservationRoomRepository>();
        services.AddScoped<IReservationRoomServiceRepository, ReservationRoomServiceRepository>();
        services.AddScoped<IReservationRoomTimelineRepository, ReservationRoomTimelineRepository>();
        services.AddScoped<IReservationRoomPaymentRepository, ReservationRoomPaymentRepository>();
        services.AddScoped<IRateRoomTypeRepository, RateRoomTypeRepository>();
        services.AddScoped<IRateRepository, RateRepository>();
        services.AddScoped<IRateTimelineRepository, RateTimelineRepository>();
        services.AddScoped<IAvailibilityTimelineRepository, AvailibilityRepository>();

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IEmailSender, SmtpEmailSender>();

        services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"] ?? string.Empty))
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
                        var result = Result<object>.Failure(AuthErrors.Unauthorized());
                        var resultJson = JsonConvert.SerializeObject(result);
                        return context.Response.WriteAsync(resultJson);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = Result<object>.Failure(AuthErrors.Forbidden());
                        var resultJson = JsonConvert.SerializeObject(result);
                        return context.Response.WriteAsync(resultJson);
                    },
                };
            });
    }
}