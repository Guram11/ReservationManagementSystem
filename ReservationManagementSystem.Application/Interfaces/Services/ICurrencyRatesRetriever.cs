using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.Application.Interfaces.Services;

public interface ICurrencyRatesRetriever
{
    Task<CurrencyRatesResponse> FetchRatesAsync(DateTime date);
}
