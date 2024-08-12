using AutoMapper;
using MediatR;
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
        var hotel = await _hotelRepository.Delete(request.Id);

        if (hotel is null)
        {
            return Result<HotelResponse>.Failure(NotFoundError.NotFound($"Hotel with ID {request.Id} was not found."));
        }

        var response = _mapper.Map<HotelResponse>(hotel);

        return Result<HotelResponse>.Success(response);
    }
}
