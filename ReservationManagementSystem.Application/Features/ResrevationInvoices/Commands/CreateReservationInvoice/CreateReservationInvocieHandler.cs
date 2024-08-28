using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;

public sealed class CreateReservationInvoicelHandler : IRequestHandler<CreateReservationInvoiceRequest, Result<ReservationInvoiceResponse>>
{
    private readonly IReservationInvoiceRepository _reservationInvoiceRepository;
    private readonly IReservationRoomTimelineRepository _roomTimelineRepository;
    private readonly IReservationRoomRepository _reservationRoomRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateReservationInvoiceRequest> _validator;

    public CreateReservationInvoicelHandler(IReservationInvoiceRepository reservationInvoiceRepository,
        IReservationRoomTimelineRepository roomTimelineRepository,
        IReservationRoomRepository reservationRoomRepository,
        IMapper mapper, IValidator<CreateReservationInvoiceRequest> validator)
    {
        _reservationInvoiceRepository = reservationInvoiceRepository;
        _roomTimelineRepository = roomTimelineRepository;
        _reservationRoomRepository = reservationRoomRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<ReservationInvoiceResponse>> Handle(CreateReservationInvoiceRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<ReservationInvoiceResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var reservationRoom = await _reservationRoomRepository.GetReservationRoomByReservationId(request.ReservationId);
        var currencyRate = await _reservationInvoiceRepository.GetCurrencyRate(request.Currency.ToString());

        if (currencyRate == null || reservationRoom == null)
        {
            return Result<ReservationInvoiceResponse>.Failure(ReservationRoomErrors.NotFound());
        }

        var roomTimeline = await _roomTimelineRepository.GetReservationRoomTimelinesByReservationRoomId(reservationRoom.Id);
        var totalAmountInGEL = roomTimeline.Sum(rt => rt.Price);

        var finalAmount = request.Currency == Currencies.GEL ? totalAmountInGEL : totalAmountInGEL / currencyRate.Rate;
        var dueAmount = finalAmount - request.Paid;

        var invoice = new ReservationInvoices
        {
            ReservationId = request.ReservationId,
            Amount = finalAmount,
            Paid = request.Paid,
            Due = dueAmount,
            Currency = request.Currency,
        };

        var reservationInvoice = _mapper.Map<ReservationInvoices>(invoice);
        await _reservationInvoiceRepository.Create(reservationInvoice);

        var reservationResponse = _mapper.Map<ReservationInvoiceResponse>(reservationInvoice);
        return Result<ReservationInvoiceResponse>.Success(reservationResponse);
    }
}
