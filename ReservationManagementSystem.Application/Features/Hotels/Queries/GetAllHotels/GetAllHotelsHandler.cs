using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;

public sealed class GetAllHotelsHandler : IRequestHandler<GetAllHotelsRequest, Result<List<HotelResponse>>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetAllHotelsHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<HotelResponse>>> Handle(GetAllHotelsRequest request, CancellationToken cancellationToken)
    {
        var hotels = await _hotelRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        var response = _mapper.Map<List<HotelResponse>>(hotels);

        return Result<List<HotelResponse>>.Success(response);
    }
}
