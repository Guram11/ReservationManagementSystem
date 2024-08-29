using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Queries.GetAllRateRoomTypes;

public sealed class GetAllRateRoomTypesHandler : IRequestHandler<GetAllRateRoomTypesRequest, Result<List<RateRoomTypeResponse>>>
{
    private readonly IRateRoomTypeRepository _rateRoomTypeRepository;
    private readonly IMapper _mapper;

    public GetAllRateRoomTypesHandler(IRateRoomTypeRepository rateRoomTypeRepository, IMapper mapper)
    {
        _rateRoomTypeRepository = rateRoomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<RateRoomTypeResponse>>> Handle(GetAllRateRoomTypesRequest request, CancellationToken cancellationToken)
    {
        var roomTypes = await _rateRoomTypeRepository.GetAll(
            request.FilterOn, request.FilterQuery, request.SortBy,
            request.IsAscending, request.PageNumber, request.PageSize, cancellationToken);

        var response = _mapper.Map<List<RateRoomTypeResponse>>(roomTypes);

        return Result<List<RateRoomTypeResponse>>.Success(response);
    }
}

