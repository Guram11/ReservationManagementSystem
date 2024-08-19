using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Queries.GetAllReservationRoomServices;

public sealed class GetAllReservationRoomServicesHandler : IRequestHandler<GetAllReservationRoomServicesRequest, List<ReservationRoomServiceResponse>>
{
    private readonly IReservationRoomServiceRepository _reservationRoomServiceRepository;
    private readonly IMapper _mapper;

    public GetAllReservationRoomServicesHandler(IReservationRoomServiceRepository reservationRoomServiceRepository, IMapper mapper)
    {
        _reservationRoomServiceRepository = reservationRoomServiceRepository;
        _mapper = mapper;
    }

    public async Task<List<ReservationRoomServiceResponse>> Handle(GetAllReservationRoomServicesRequest request, CancellationToken cancellationToken)
    {
        var reservationRoomServices = await _reservationRoomServiceRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<ReservationRoomServiceResponse>>(reservationRoomServices);
    }
}
