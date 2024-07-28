using FluentValidation;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.UpdateGuest;

public sealed class UpdateGuestValidator : AbstractValidator<UpdateGuestRequest>
{
    public UpdateGuestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(x => x.PhoneNumber).NotEmpty().MinimumLength(3).MaximumLength(50);
    }
}
