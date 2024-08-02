using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.UpdateGuest;

public sealed class UpdateGuestHandler : IRequestHandler<UpdateGuestRequest, GuestResponse>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateGuestRequest> _validator;

    public UpdateGuestHandler(IGuestRepository guestRepository, IMapper mapper, IValidator<UpdateGuestRequest> validator)
    {
        _guestRepository = guestRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<GuestResponse> Handle(UpdateGuestRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException($"{errors}");
        }

        var guest = _mapper.Map<Guest>(request);
        await _guestRepository.Update(request.Id, guest);

        return _mapper.Map<GuestResponse>(guest);
    }
}
