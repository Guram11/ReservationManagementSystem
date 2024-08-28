using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Queries.GetRoomById;

public sealed class GetRoomByIdHandler : IRequestHandler<GetRoomByIdRequest, Result<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomByIdHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<RoomResponse>> Handle(GetRoomByIdRequest request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.Get(request.Id);

        if (room is null)
        {
            return Result<RoomResponse>.Failure(RoomErrors.NotFound(request.Id));
        }

        var response = _mapper.Map<RoomResponse>(room);
        return Result<RoomResponse>.Success(response);
    }
}
