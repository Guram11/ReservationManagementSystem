using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.DeleteGuest;

public sealed class DeleteGuestHandler : IRequestHandler<DeleteGuestRequest, GuestResponse>
{
    private readonly IGuestRepository _userRepository;
    private readonly IMapper _mapper;

    public DeleteGuestHandler(IGuestRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GuestResponse> Handle(DeleteGuestRequest request, CancellationToken cancellationToken)
    {
        var guest = await _userRepository.Delete(request.Id);

        return _mapper.Map<GuestResponse>(guest);
    }
}
