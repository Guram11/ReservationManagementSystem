using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Commands.CreateReservationRoomPayment;

public sealed class CreateReservationRoomPaymentHandler : IRequestHandler<CreateReservationRoomPaymentRequest, Result<ReservationRoomPaymentsResponse>>
{
    private readonly IReservationRoomPaymentRepository _reservationRoomPaymentRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateReservationRoomPaymentRequest> _validator;

    public CreateReservationRoomPaymentHandler(IReservationRoomPaymentRepository reservationRoomPaymentRepository,
        IMapper mapper, IValidator<CreateReservationRoomPaymentRequest> validator)
    {
        _reservationRoomPaymentRepository = reservationRoomPaymentRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<ReservationRoomPaymentsResponse>> Handle(CreateReservationRoomPaymentRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<ReservationRoomPaymentsResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var reservationRoomPayment = _mapper.Map<ReservationRoomPayments>(request);
        await _reservationRoomPaymentRepository.Create(reservationRoomPayment, cancellationToken);

        var reservationResponse = _mapper.Map<ReservationRoomPaymentsResponse>(reservationRoomPayment);
        return Result<ReservationRoomPaymentsResponse>.Success(reservationResponse);
    }
}