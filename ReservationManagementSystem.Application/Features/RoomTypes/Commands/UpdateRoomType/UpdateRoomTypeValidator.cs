using FluentValidation;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;

public sealed class UpdateRoomTypeValidator : AbstractValidator<UpdateRoomTypeRequest>
{
    public UpdateRoomTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3).WithMessage($"Name must be at least 3 characters.")
            .MaximumLength(50);
    }
}
