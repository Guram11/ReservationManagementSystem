using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;

public sealed class UpdateRateValidator : AbstractValidator<UpdateRateRequest>
{
    public UpdateRateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3).WithMessage($"Name must be at least 3 characters.")
            .MaximumLength(50);
    }
}
