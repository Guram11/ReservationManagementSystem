using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.UpdateRoom;

public sealed class UpdateRoomValidator : AbstractValidator<UpdateRoomRequest>
{
    public UpdateRoomValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty()
            .MinimumLength(3).WithMessage($"Number must be at least 3 characters.")
            .MaximumLength(50);
    }
}
