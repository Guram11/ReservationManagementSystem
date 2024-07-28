using FluentValidation;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.CreateGuest;

public sealed class CreateGuestValidator : AbstractValidator<CreateGuestRequest>
{
    public CreateGuestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}
