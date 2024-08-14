using FluentValidation;

namespace ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;

public sealed class CreateHotelServiceValidator : AbstractValidator<CreateHotelServiceRequest>
{
    public CreateHotelServiceValidator()
    {
        RuleFor(x => x.ServiceTypeId)
            .IsInEnum()
            .WithMessage("Invalid service type.");
    }
}

