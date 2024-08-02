using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;

public sealed class CreateRoomValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty()
            .MinimumLength(3).WithMessage($"Name must be at least 3 characters.")
            .MaximumLength(50);
    }
}
