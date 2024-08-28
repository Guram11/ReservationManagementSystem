using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;

public static class ReservationInvoiceErrors
{
    public static Error NotFound(Guid id) => new Error(
        "NotFound", $"ReservationInvoice with ID {id} was not found.");
}
