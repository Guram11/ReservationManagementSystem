using AutoMapper;
using FluentValidation;
using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;

public sealed class CreateHotelHandler : IRequestHandler<CreateHotelRequest, HotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateHotelRequest> _validator;

    public CreateHotelHandler(IHotelRepository hostRepository, IMapper mapper, IValidator<CreateHotelRequest> validator)
    {
        _hotelRepository = hostRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<HotelResponse> Handle(CreateHotelRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException($"{errors}");
        }

        var hotel = _mapper.Map<Hotel>(request);
        await _hotelRepository.Create(hotel);

        return _mapper.Map<HotelResponse>(hotel);
    }
}
