using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;

public sealed class DeleteHotelHandler : IRequestHandler<DeleteHotelRequest, HotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public DeleteHotelHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<HotelResponse> Handle(DeleteHotelRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.Delete(request.Id);

        return _mapper.Map<HotelResponse>(hotel);
    }
}
