using FluentValidation;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;

public sealed class CreateRoomTypeValidator : AbstractValidator<CreateRoomTypeRequest>
{
    public CreateRoomTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3).WithMessage($"Name must be at least 3 characters.")
            .MaximumLength(50);
    }
}
