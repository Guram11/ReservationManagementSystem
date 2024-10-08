﻿using FluentValidation;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;

public sealed class UpdateHotelValidator : AbstractValidator<UpdateHotelRequest>
{
    public UpdateHotelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(5).WithMessage($"Name must be at least 5 characters.")
            .MaximumLength(50);
    }
}
