using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;

public sealed class CreateUserHandler : IRequestHandler<CreateGuestRequest, GuestResponse>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public CreateUserHandler(IGuestRepository guestRepository, IMapper mapper)
    {
        _guestRepository = guestRepository;
        _mapper = mapper;
    }

    public async Task<GuestResponse> Handle(CreateGuestRequest request, CancellationToken cancellationToken)
    {
        var guest = _mapper.Map<Guest>(request);
        await _guestRepository.Create(guest);

        return _mapper.Map<GuestResponse>(guest);
    }
}