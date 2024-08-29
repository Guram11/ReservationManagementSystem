using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.DeleteReservationRoomPayment;

public sealed class DeleteReservationRoomPaymentHandler : IRequestHandler<DeleteReservationRoomPaymentRequest, Result<ReservationRoomPaymentsResponse>>
{
    private readonly IReservationRoomPaymentRepository _reservationRoomRepository;
    private readonly IMapper _mapper;

    public DeleteReservationRoomPaymentHandler(IReservationRoomPaymentRepository roomTypeRepository, IMapper mapper)
    {
        _reservationRoomRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationRoomPaymentsResponse>> Handle(DeleteReservationRoomPaymentRequest request, CancellationToken cancellationToken)
    {
        var reservationRoomPayment = await _reservationRoomRepository.Delete(request.Id, cancellationToken);

        if (reservationRoomPayment == null)
        {
            return Result<ReservationRoomPaymentsResponse>.Failure(ReservationRoomPaymentErrors.NotFound(request.Id));
        }

        var response = _mapper.Map<ReservationRoomPaymentsResponse>(reservationRoomPayment);
        return Result<ReservationRoomPaymentsResponse>.Success(response);
    }
}
