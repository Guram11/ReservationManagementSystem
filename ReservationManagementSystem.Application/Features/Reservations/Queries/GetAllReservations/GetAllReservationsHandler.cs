using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Reservations.Queries.GetAllReservations;

public sealed class GetAllReservationsHandler : IRequestHandler<GetAllReservationsRequest, List<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;

    public GetAllReservationsHandler(IReservationRepository reservationRepository, IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<List<ReservationResponse>> Handle(GetAllReservationsRequest request, CancellationToken cancellationToken)
    {
        var reservations = await _reservationRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<ReservationResponse>>(reservations);
    }
}
