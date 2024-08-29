using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;

public sealed class CreateHotelServicelHandler : IRequestHandler<CreateHotelServiceRequest, Result<HotelServiceResponse>>
{
    private readonly IHotelServiceRepository _hotelServiceRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateHotelServiceRequest> _validator;

    public CreateHotelServicelHandler(IHotelServiceRepository hotelServiceRepository, IMapper mapper, IValidator<CreateHotelServiceRequest> validator)
    {
        _hotelServiceRepository = hotelServiceRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<HotelServiceResponse>> Handle(CreateHotelServiceRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<HotelServiceResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var hotelService = _mapper.Map<HotelService>(request);
        await _hotelServiceRepository.Create(hotelService, cancellationToken);

        var hotelServiceResponse = _mapper.Map<HotelServiceResponse>(hotelService);
        return Result<HotelServiceResponse>.Success(hotelServiceResponse);
    }
}

