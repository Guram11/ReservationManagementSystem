using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public sealed class GetRoomTypeByIdHandler : IRequestHandler<GetRoomTypeByIdRequest, RoomTypeResponse>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;

    public GetRoomTypeByIdHandler(IRoomTypeRepository roomRepository, IMapper mapper)
    {
        _roomTypeRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<RoomTypeResponse> Handle(GetRoomTypeByIdRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _roomTypeRepository.Get(request.Id, cancellationToken);
        return _mapper.Map<RoomTypeResponse>(hotel);
    }
}