using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;

public sealed class GetAllHotelsHandler : IRequestHandler<GetAllHotelsRequest, List<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetAllHotelsHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<List<HotelResponse>> Handle(GetAllHotelsRequest request, CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<HotelResponse>>(hotels);
    }
}
