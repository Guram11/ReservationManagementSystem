using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Rooms.Commands.DeleteRoom;

public sealed class DeleteRoomHandler : IRequestHandler<DeleteRoomRequest, RoomResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public DeleteRoomHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<RoomResponse> Handle(DeleteRoomRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _roomRepository.Delete(request.Id);

        return _mapper.Map<RoomResponse>(hotel);
    }
}
