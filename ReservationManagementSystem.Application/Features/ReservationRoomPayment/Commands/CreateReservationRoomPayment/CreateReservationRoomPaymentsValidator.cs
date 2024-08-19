using FluentValidation;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.CreateReservationRoomPayment;

public sealed class CreateReservationRoomPaymentsValidator : AbstractValidator<CreateReservationRoomPaymentRequest>
{
    public CreateReservationRoomPaymentsValidator()
    {
        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithMessage("Invalid currency type.");
    }
}
