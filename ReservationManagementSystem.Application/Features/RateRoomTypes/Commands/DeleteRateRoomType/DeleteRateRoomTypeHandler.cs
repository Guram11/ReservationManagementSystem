using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;

public sealed class DeleteRateRoomTypeHandler : IRequestHandler<DeleteRateRoomTypeRequest, Result<RateRoomTypeResponse>>
{
    private readonly IRateRoomTypeRepository _rateRoomTypeRepository;
    private readonly IMapper _mapper;

    public DeleteRateRoomTypeHandler(IRateRoomTypeRepository roomTypeRepository, IMapper mapper)
    {
        _rateRoomTypeRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<RateRoomTypeResponse>> Handle(DeleteRateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var rateRoomType = await _rateRoomTypeRepository.Delete(request.RateId, request.RoomTypeId);

        if (rateRoomType == null)
        {
            return Result<RateRoomTypeResponse>.Failure(RateRoomTypeErrors.NotFound());
        }

        var response = _mapper.Map<RateRoomTypeResponse>(rateRoomType);
        return Result<RateRoomTypeResponse>.Success(response);
    }
}

