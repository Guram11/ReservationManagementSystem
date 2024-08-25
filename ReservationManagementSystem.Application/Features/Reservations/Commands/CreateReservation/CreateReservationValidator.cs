using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;

public sealed class CreateResevationValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateResevationValidator()
    {
        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithMessage("Invalid currency type.");
    }
}