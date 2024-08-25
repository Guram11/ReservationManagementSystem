using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class CurrencyRate : BaseEntity
{
    public string MainCurrency { get; set; } = "GEL";
    public required string Currency { get; set; }
    public required string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
}
