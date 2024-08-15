using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.DeleteReservationInvoice;

public sealed class DeleteReservationInvoiceHandler : IRequestHandler<DeleteReservationInvoiceRequest, Result<ReservationInvoiceResponse>>
{
    private readonly IReservationInvoiceRepository _reservationInvoiceRepository;
    private readonly IMapper _mapper;

    public DeleteReservationInvoiceHandler(IReservationInvoiceRepository reservationInvoiceRepository, IMapper mapper)
    {
        _reservationInvoiceRepository = reservationInvoiceRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationInvoiceResponse>> Handle(DeleteReservationInvoiceRequest request, CancellationToken cancellationToken)
    {
        var reservationInvoice = await _reservationInvoiceRepository.Delete(request.Id);

        if (reservationInvoice is null)
        {
            return Result<ReservationInvoiceResponse>.Failure(NotFoundError.NotFound($"Reservation Invoice with ID {request.Id} was not found."));
        }

        var response = _mapper.Map<ReservationInvoiceResponse>(reservationInvoice);

        return Result<ReservationInvoiceResponse>.Success(response);
    }
}

