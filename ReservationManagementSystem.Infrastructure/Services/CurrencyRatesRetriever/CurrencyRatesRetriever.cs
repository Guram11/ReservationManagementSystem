using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Infrastructure.Context;
using ReservationManagementSystem.Infrastructure.Helpers;

namespace ReservationManagementSystem.Infrastructure.Services.CurrencyRatesRetriever;

public class CurrencyRatesService : IHostedService, IDisposable, ICurrencyRatesRetriever
{
    private readonly ILogger<CurrencyRatesService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly CurrencyRatesSettings _currencyRateSettings;
    private Timer? _timer = null;
    private static CurrencyRatesResponse? _currentRates;
    private DateTime _lastFetchTime;

    public CurrencyRatesService(
        ILogger<CurrencyRatesService> logger,
        IHttpClientFactory httpClientFactory,
        IServiceScopeFactory scopeFactory,
        IOptions<CurrencyRatesSettings> currencyRateSettings)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _scopeFactory = scopeFactory;
        _currencyRateSettings = currencyRateSettings.Value;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting CurrencyRatesService.");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        try
        {
            var now = DateTime.UtcNow;

            _currentRates = await FetchRatesAsync();
            _lastFetchTime = now;

            if (_currentRates != null)
            {
                await SaveRatesToDatabase(_currentRates, _lastFetchTime);
            }

            _logger.LogInformation("Fetched new currency rates and stored them in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching currency rates.");
        }
    }

    private async Task SaveRatesToDatabase(CurrencyRatesResponse rates, DateTime retrievedAt)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        dbContext.CurrencyRates.RemoveRange(dbContext.CurrencyRates);

        if (rates.Rates != null && rates.MainCurrency !=null)
        {
            foreach (var rate in rates.Rates)
            {
                var currencyRate = new CurrencyRate
                {
                    MainCurrency = rates.MainCurrency,
                    Currency = rate.Key,
                    CurrencyCode = rate.Key,
                    Rate = rate.Value,
                    UpdatedAt = retrievedAt
                };

                dbContext.CurrencyRates.Add(currencyRate);
            }

            await dbContext.SaveChangesAsync();
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stopping CurrencyRatesService.");
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task<CurrencyRatesResponse> FetchRatesAsync()
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync($"{_currencyRateSettings.Url}{DateTime.Now}");
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();

        return DeserializeCurrencyRatesResponse.DeserializeXmlResponse(data);
    }   
}
