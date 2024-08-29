using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.HotelServices.Queries;

public sealed class GetAllHotelServicesHandler : IRequestHandler<GetAllHotelServicesRequest, Result<List<HotelServiceResponse>>>
{
    private readonly IHotelServiceRepository _hoteServicelRepository;
    private readonly IMapper _mapper;

    public GetAllHotelServicesHandler(IHotelServiceRepository hotelRepository, IMapper mapper)
    {
        _hoteServicelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<HotelServiceResponse>>> Handle(GetAllHotelServicesRequest request, CancellationToken cancellationToken)
    {
        var hotels = await _hoteServicelRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, cancellationToken);

        var response = _mapper.Map<List<HotelServiceResponse>>(hotels);

        return Result<List<HotelServiceResponse>>.Success(response);
    }
}
