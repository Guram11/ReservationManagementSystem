using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;

public sealed class CreateReservationInvoicelHandler : IRequestHandler<CreateReservationInvoiceRequest, Result<ReservationInvoiceResponse>>
{
    private readonly IReservationInvoiceRepository _reservationInvoiceRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateReservationInvoiceRequest> _validator;

    public CreateReservationInvoicelHandler(IReservationInvoiceRepository reservationInvoiceRepository, IMapper mapper, IValidator<CreateReservationInvoiceRequest> validator)
    {
        _reservationInvoiceRepository = reservationInvoiceRepository;
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

        var reservationInvoice = _mapper.Map<ReservationInvoices>(request);
        await _reservationInvoiceRepository.Create(reservationInvoice);

        var reservationResponse = _mapper.Map<ReservationInvoiceResponse>(reservationInvoice);
        return Result<ReservationInvoiceResponse>.Success(reservationResponse);
    }
}
