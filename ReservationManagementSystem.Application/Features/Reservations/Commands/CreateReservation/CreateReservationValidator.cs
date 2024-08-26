using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;

public sealed class CreateResevationValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateResevationValidator()
    {
        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithMessage("Invalid currency type.");
        RuleFor(x => x.NumberOfRooms)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Number of rooms must be at least 1");
    }
}