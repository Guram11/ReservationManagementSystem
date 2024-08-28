using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Queries.GetAllRooms;

public sealed class GetAllRoomsHandler : IRequestHandler<GetAllRoomsRequest, Result<List<RoomResponse>>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetAllRoomsHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<RoomResponse>>> Handle(GetAllRoomsRequest request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        var response = _mapper.Map<List<RoomResponse>>(rooms);

        return Result<List<RoomResponse>>.Success(response);
    }
}

