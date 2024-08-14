using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.HotelServices.Queries;

public sealed class GetAllHotelServicesHandler : IRequestHandler<GetAllHotelServicesRequest, List<HotelServiceResponse>>
{
    private readonly IHotelServiceRepository _hoteServicelRepository;
    private readonly IMapper _mapper;

    public GetAllHotelServicesHandler(IHotelServiceRepository hotelRepository, IMapper mapper)
    {
        _hoteServicelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<List<HotelServiceResponse>> Handle(GetAllHotelServicesRequest request, CancellationToken cancellationToken)
    {
        var hotels = await _hoteServicelRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize);

        return _mapper.Map<List<HotelServiceResponse>>(hotels);
    }
}
