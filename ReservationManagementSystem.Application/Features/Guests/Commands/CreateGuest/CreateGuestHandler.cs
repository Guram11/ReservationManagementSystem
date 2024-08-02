using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;

public sealed class CreateGuestHandler : IRequestHandler<CreateGuestRequest, GuestResponse>
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

    public async Task<GuestResponse> Handle(CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException($"{errors}");
        }

        var guest = _mapper.Map<Guest>(request);
        await _guestRepository.Create(guest);

        return _mapper.Map<GuestResponse>(guest);
    }
}