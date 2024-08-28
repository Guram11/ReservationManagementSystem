using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Commands.DeleteGuest;

public sealed class DeleteGuestHandler : IRequestHandler<DeleteGuestRequest, Result<GuestResponse>>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public DeleteGuestHandler(IGuestRepository userRepository, IMapper mapper)
    {
        _guestRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<GuestResponse>> Handle(DeleteGuestRequest request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.Delete(request.Id);

        if (guest is null)
        {
            return Result<GuestResponse>.Failure(GuestErrors.NotFound(request.Id));
        }

        var response = _mapper.Map<GuestResponse>(guest);
        return Result<GuestResponse>.Success(response);
    }
}
