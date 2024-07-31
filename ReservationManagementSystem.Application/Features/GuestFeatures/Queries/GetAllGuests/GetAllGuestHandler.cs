using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.GuestFeatures.Queries.GetAllGuests;

public sealed class GetAllUserHandler : IRequestHandler<GetAllUserRequest, List<GuestResponse>>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public GetAllUserHandler(IGuestRepository userRepository, IMapper mapper)
    {
        _guestRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<GuestResponse>> Handle(GetAllUserRequest request, CancellationToken cancellationToken)
    {
        var users = await _guestRepository.GetAll(cancellationToken,
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<GuestResponse>>(users);
    }
}
