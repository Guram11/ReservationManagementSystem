﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;

public sealed class DeleteHotelHandler : IRequestHandler<DeleteHotelRequest, Result<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public DeleteHotelHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<Result<HotelResponse>> Handle(DeleteHotelRequest request, CancellationToken cancellationToken)
    {
        var isInUse = await _hotelRepository.IsHotelInUseAsync(request.Id);
        if (isInUse)
        {
            return Result<HotelResponse>.Failure(ValidationError.ResourceInUse());
        }

        var hotel = await _hotelRepository.Delete(request.Id, cancellationToken);

        if (hotel is null)
        {
            return Result<HotelResponse>.Failure(HotelErrors.NotFound(request.Id));
        }

        var response = _mapper.Map<HotelResponse>(hotel);
        return Result<HotelResponse>.Success(response);
    }
}
