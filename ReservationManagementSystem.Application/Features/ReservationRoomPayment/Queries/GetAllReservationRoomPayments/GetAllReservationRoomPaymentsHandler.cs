using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Queries.GetAllReservationRoomPayments;

public sealed class GetAllRateReservationRoomPaymentsHandler : IRequestHandler<GetAllReservationRoomPaymentsRequest, Result<List<ReservationRoomPaymentsResponse>>>
{
    private readonly IReservationRoomPaymentRepository _reservationRoomRepository;
    private readonly IMapper _mapper;

    public GetAllRateReservationRoomPaymentsHandler(IReservationRoomPaymentRepository rateRoomTypeRepository, IMapper mapper)
    {
        _reservationRoomRepository = rateRoomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<ReservationRoomPaymentsResponse>>> Handle(GetAllReservationRoomPaymentsRequest request, CancellationToken cancellationToken)
    {
        var reservationRooms = await _reservationRoomRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, cancellationToken);

        var response = _mapper.Map<List<ReservationRoomPaymentsResponse>>(reservationRooms);

        return Result<List<ReservationRoomPaymentsResponse>>.Success(response); 
    }
}
