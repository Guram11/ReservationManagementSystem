using FluentValidation;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;

public sealed class CreateResevationInvoiceValidator : AbstractValidator<CreateReservationInvoiceRequest>
{
    public CreateResevationInvoiceValidator()
    {
        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithMessage("Invalid currency type.");
    }
}
