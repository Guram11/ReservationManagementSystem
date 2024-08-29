using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Infrastructure.Context;

namespace ReservationManagementSystem.Infrastructure.Persistence.Repositories;

public class ReservationInvoiceRepository : BaseRepository<ReservationInvoices>, IReservationInvoiceRepository
{
    private readonly DataContext _context;

    public ReservationInvoiceRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CurrencyRate?> GetCurrencyRate(string currency, CancellationToken cancellationToken)
    {
        var currencyRate = await _context.CurrencyRates.FirstOrDefaultAsync(c => c.Currency == currency, cancellationToken);

        if (currencyRate == null)
        {
            return null;
        }

        return currencyRate;
    }
}
