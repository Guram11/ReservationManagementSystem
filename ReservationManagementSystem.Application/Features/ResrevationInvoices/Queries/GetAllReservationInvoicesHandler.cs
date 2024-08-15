using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Queries;

public sealed class GetAllReservationInvoicesHandler : IRequestHandler<GetAllReservationInvoicesRequest, List<ReservationInvoiceResponse>>
{
    private readonly IReservationInvoiceRepository _reservationInvoiceRepository;
    private readonly IMapper _mapper;

    public GetAllReservationInvoicesHandler(IReservationInvoiceRepository reservationRepository, IMapper mapper)
    {
        _reservationInvoiceRepository = reservationRepository;
        _mapper = mapper;
    }

    public async Task<List<ReservationInvoiceResponse>> Handle(GetAllReservationInvoicesRequest request, CancellationToken cancellationToken)
    {
        var reservationInvoices = await _reservationInvoiceRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<ReservationInvoiceResponse>>(reservationInvoices);
    }
}
