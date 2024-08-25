using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;

public sealed class CreateHotelValidator : AbstractValidator<CreateHotelRequest>
{
    public CreateHotelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(5).WithMessage($"Name must be at least 5 characters.");
    }
}
