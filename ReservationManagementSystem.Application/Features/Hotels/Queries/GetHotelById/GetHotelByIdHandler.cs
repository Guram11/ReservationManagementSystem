using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetHotelById;

public sealed class GetHotelByIdHandler : IRequestHandler<GetHotelByIdRequest, HotelResponse>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelByIdHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<HotelResponse> Handle(GetHotelByIdRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.Get(request.Id, cancellationToken);
        return _mapper.Map<HotelResponse>(hotel);
    }
}
