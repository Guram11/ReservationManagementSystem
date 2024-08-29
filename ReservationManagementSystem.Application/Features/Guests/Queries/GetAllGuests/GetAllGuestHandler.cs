using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Guests.Queries.GetAllGuests;

public sealed class GetAllUserHandler : IRequestHandler<GetAllGuestsRequest, Result<List<GuestResponse>>>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IMapper _mapper;

    public GetAllUserHandler(IGuestRepository userRepository, IMapper mapper)
    {
        _guestRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<GuestResponse>>> Handle(GetAllGuestsRequest request, CancellationToken cancellationToken)
    {
        var users = await _guestRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, cancellationToken);

        var response = _mapper.Map<List<GuestResponse>>(users);

        return Result<List<GuestResponse>>.Success(response);
    }
}
