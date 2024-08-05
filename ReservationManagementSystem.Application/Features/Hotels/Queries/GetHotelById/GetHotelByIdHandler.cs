using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Hotels.Queries.GetHotelById;

public sealed class GetHotelByIdHandler : IRequestHandler<GetHotelByIdRequest, Result<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public GetHotelByIdHandler(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<Result<HotelResponse>> Handle(GetHotelByIdRequest request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.Get(request.Id);
        var response =  _mapper.Map<HotelResponse>(hotel);

        return Result<HotelResponse>.Success(response);
    }
}
