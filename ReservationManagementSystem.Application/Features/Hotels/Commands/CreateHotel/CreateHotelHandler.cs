using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;

public sealed class CreateHotelHandler : IRequestHandler<CreateHotelRequest, Result<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateHotelRequest> _validator;

    public CreateHotelHandler(IHotelRepository hotelRepository, IMapper mapper, IValidator<CreateHotelRequest> validator)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<HotelResponse>> Handle(CreateHotelRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            foreach (var error in errors)
            {
                return Result<HotelResponse>.Failure(ValidationError.ValidationFailed(error));
            }
        }

        var hotel = _mapper.Map<Hotel>(request);
        await _hotelRepository.Create(hotel);

        var hotelResponse = _mapper.Map<HotelResponse>(hotel);
        return Result<HotelResponse>.Success(hotelResponse);
    }
}
