using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;

public sealed class UpdateHotelHandler : IRequestHandler<UpdateHotelRequest, Result<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateHotelRequest> _validator;

    public UpdateHotelHandler(IHotelRepository hotelRepository, IMapper mapper, IValidator<UpdateHotelRequest> validator)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<HotelResponse>> Handle(UpdateHotelRequest request, CancellationToken cancellationToken)
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
        await _hotelRepository.Update(request.Id, hotel);

        var response = _mapper.Map<HotelResponse>(hotel);
        return Result<HotelResponse>.Success(response);
    }
}
