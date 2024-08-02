using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Features.Rooms.Queries.GetAllRooms;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetAllRoomTypes;

public sealed class GetAllRoomTypesHandler : IRequestHandler<GetAllRoomTypesRequest, List<RoomTypeResponse>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;

    public GetAllRoomTypesHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper)
    {
        _roomTypeRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<List<RoomTypeResponse>> Handle(GetAllRoomTypesRequest request, CancellationToken cancellationToken)
    {
        var roomTypes = await _roomTypeRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<RoomTypeResponse>>(roomTypes);
    }
}
