using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;

public sealed class CreateGuestHandler : IRequestHandler<CreateGuestRequest, Result<GuestResponse>>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateGuestRequest> _validator;

    public CreateGuestHandler(IGuestRepository guestRepository, IMapper mapper, IValidator<CreateGuestRequest> validator)
    {
        _guestRepository = guestRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<GuestResponse>> Handle(CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<GuestResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var createdGuest = await _guestRepository.GetGuestByEmail(request.Email);

        if (createdGuest != null)
        {
            return Result<GuestResponse>.Failure(AlreadyCreatedError.AlreadyCreated("Email is already in use!"));
        }

        var guest = _mapper.Map<Guest>(request);
        await _guestRepository.Create(guest);

        var guestResponse = _mapper.Map<GuestResponse>(guest);
        return Result<GuestResponse>.Success(guestResponse);
    }
}