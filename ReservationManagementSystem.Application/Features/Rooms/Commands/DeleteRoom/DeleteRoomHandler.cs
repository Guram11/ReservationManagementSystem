using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.DeleteRoom;

public sealed class DeleteRoomHandler : IRequestHandler<DeleteRoomRequest, Result<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public DeleteRoomHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<Result<RoomResponse>> Handle(DeleteRoomRequest request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.Delete(request.Id);
        var roomResponse = _mapper.Map<RoomResponse>(room);

        return Result<RoomResponse>.Success(roomResponse);
    }
}
