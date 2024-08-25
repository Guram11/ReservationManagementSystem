using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Interfaces.Repositories;

public interface IReservationInvoiceRepository : IBaseRepository<ReservationInvoices>
{
    Task<CurrencyRate?> GetCurrencyRate(string currency);
}
