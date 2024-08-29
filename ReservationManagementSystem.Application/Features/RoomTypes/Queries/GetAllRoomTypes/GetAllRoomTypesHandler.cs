using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetAllRoomTypes;

public sealed class GetAllRoomTypesHandler : IRequestHandler<GetAllRoomTypesRequest, Result<List<RoomTypeResponse>>>
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IMapper _mapper;

    public GetAllRoomTypesHandler(IRoomTypeRepository roomTypeRepository, IMapper mapper)
    {
        _roomTypeRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<RoomTypeResponse>>> Handle(GetAllRoomTypesRequest request, CancellationToken cancellationToken)
    {
        var roomTypes = await _roomTypeRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, cancellationToken);

        var response = _mapper.Map<List<RoomTypeResponse>>(roomTypes);

        return Result<List<RoomTypeResponse>>.Success(response);
    }
}
