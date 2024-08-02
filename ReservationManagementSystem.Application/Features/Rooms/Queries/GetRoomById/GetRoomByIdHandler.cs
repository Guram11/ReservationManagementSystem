using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Rooms.Queries.GetRoomById;

public sealed class GetRoomByIdHandler : IRequestHandler<GetRoomByIdRequest, RoomResponse>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public GetRoomByIdHandler(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<RoomResponse> Handle(GetRoomByIdRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _roomRepository.Get(request.Id, cancellationToken);
        return _mapper.Map<RoomResponse>(hotel);
    }
}
