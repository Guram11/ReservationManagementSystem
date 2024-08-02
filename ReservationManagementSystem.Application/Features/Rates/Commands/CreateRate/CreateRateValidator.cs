using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.CreateRate;

public sealed class CreateRateValidator : AbstractValidator<CreateRateRequest>
{
    public CreateRateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3).WithMessage($"Name must be at least 3 characters.")
            .MaximumLength(50);
    }
}
