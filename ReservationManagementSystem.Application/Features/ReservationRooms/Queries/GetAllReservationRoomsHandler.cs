using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.ReservationRooms.Queries;

public sealed class GetAllRateReservationRoomsHandler : IRequestHandler<GetAllReservationRoomsRequest, List<ReservationRoomResponse>>
{
    private readonly IReservationRoomRepository _reservationRoomRepository;
    private readonly IMapper _mapper;

    public GetAllRateReservationRoomsHandler(IReservationRoomRepository rateRoomTypeRepository, IMapper mapper)
    {
        _reservationRoomRepository = rateRoomTypeRepository;
        _mapper = mapper;
    }

    public async Task<List<ReservationRoomResponse>> Handle(GetAllReservationRoomsRequest request, CancellationToken cancellationToken)
    {
        var reservationRooms = await _reservationRoomRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<ReservationRoomResponse>>(reservationRooms);
    }
}
