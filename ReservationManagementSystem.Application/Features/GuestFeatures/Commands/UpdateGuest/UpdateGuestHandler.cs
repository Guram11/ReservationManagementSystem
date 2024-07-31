using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.Commands.UpdateGuest;

public sealed class UpdateGuestHandler : IRequestHandler<UpdateGuestRequest, GuestResponse>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public UpdateGuestHandler(IGuestRepository guestRepository, IMapper mapper)
    {
        _guestRepository = guestRepository;
        _mapper = mapper;
    }

    public async Task<GuestResponse> Handle(UpdateGuestRequest request, CancellationToken cancellationToken)
    {
        var guest = _mapper.Map<Guest>(request);
        await _guestRepository.Update(request.Id, guest);

        return _mapper.Map<GuestResponse>(guest);
    }
}
