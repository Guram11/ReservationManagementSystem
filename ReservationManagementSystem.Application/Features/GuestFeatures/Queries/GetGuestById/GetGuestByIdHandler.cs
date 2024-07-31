using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.Queries.GetGuestById;

public sealed class GetGuestByIdHandler : IRequestHandler<GetGuestByIdRequest, GuestResponse>
{
    private readonly IGuestRepository _userRepository;
    private readonly IMapper _mapper;

    public GetGuestByIdHandler(IGuestRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GuestResponse> Handle(GetGuestByIdRequest request, CancellationToken cancellationToken)
    {
        var guest = await _userRepository.Get(request.Id, cancellationToken);
        return _mapper.Map<GuestResponse>(guest);
    }
}
