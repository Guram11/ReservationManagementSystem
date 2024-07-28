using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;
using ReservationManagementSystem.Application.Repositories;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.GetAllGuests;

public sealed class GetAllUserHandler : IRequestHandler<GetAllUserRequest, List<GuestResponse>>
{
    private readonly IGuestRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUserHandler(IGuestRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<GuestResponse>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll(cancellationToken);
        return _mapper.Map<List<GuestResponse>>(users);
    }
}
