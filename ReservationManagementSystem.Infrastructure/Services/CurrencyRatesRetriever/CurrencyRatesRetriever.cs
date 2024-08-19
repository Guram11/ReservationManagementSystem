using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Infrastructure.Helpers;
using System.Xml.Linq;

namespace ReservationManagementSystem.Infrastructure.Services.CurrencyRatesRetriever;

internal class CurrencyRatesService(ILogger<CurrencyRatesService> logger, IHttpClientFactory httpClientFactory) : IHostedService, IDisposable, ICurrencyRatesRetriever
{
    private readonly ILogger<CurrencyRatesService> _logger = logger;
    private Timer? _timer = null;
    private static CurrencyRatesResponse? _currentRates;
    private DateTime _lastFetchTime;

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

            _currentRates = await FetchRatesAsync(DateTime.Now);
            _lastFetchTime = now;

            if (_currentRates != null)
            {
                WriteDataToFile.WriteCurrenciesToFile(_currentRates, _lastFetchTime);
            }

            _logger.LogInformation("Fetched new currency rates and wrote to file.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching currency rates.");
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

    public async Task<CurrencyRatesResponse> FetchRatesAsync(DateTime date)
    {
        string url = $"https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/rss?date={date}";

        using HttpClient client = httpClientFactory.CreateClient();

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string data = await response.Content.ReadAsStringAsync();

        return DeserializeXmlResponse(data);
    }

    public static CurrencyRatesResponse DeserializeXmlResponse(string data)
    {
        var currencyRates = new Dictionary<string, decimal>();

        var xDoc = XDocument.Parse(data);

        var descriptionElement = xDoc?.Descendants("item")?.FirstOrDefault()?.Descendants("description").FirstOrDefault();

        if (descriptionElement != null)
        {
            var cdataContent = descriptionElement.Value;

            var startIndex = cdataContent.IndexOf("<table");
            var endIndex = cdataContent.IndexOf("</table>") + "</table>".Length;

            if (startIndex >= 0 && endIndex > startIndex)
            {
                var tableContent = cdataContent.Substring(startIndex, endIndex - startIndex);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(tableContent);

                foreach (var row in htmlDoc.DocumentNode.SelectNodes("//tr"))
                {
                    var cells = row.SelectNodes("td").ToList();
                    if (cells.Count >= 3)
                    {
                        var currencyCode = cells[0].InnerText.Trim();
                        var rate = decimal.Parse(cells[2].InnerText.Trim());

                        currencyRates[currencyCode] = rate;
                    }
                }
            }
        }

        return new CurrencyRatesResponse
        {
            MainCurrency = "GEL",
            Rates = currencyRates
        };
    }

    public static CurrencyRatesResponse? CurrentRates
    {
        get => _currentRates;
    }
}
