using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;

public sealed class DeleteRateRoomTypeHandler : IRequestHandler<DeleteRateRoomTypeRequest, RateRoomTypeResponse>
{
    private readonly IRateRoomTypeRepository _rateRoomTypeRepository;
    private readonly IMapper _mapper;

    public DeleteRateRoomTypeHandler(IRateRoomTypeRepository roomTypeRepository, IMapper mapper)
    {
        _rateRoomTypeRepository = roomTypeRepository;
        _mapper = mapper;
    }

    public async Task<RateRoomTypeResponse> Handle(DeleteRateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var rateRoomType = await _rateRoomTypeRepository.Delete(request.Id);

        return _mapper.Map<RateRoomTypeResponse>(rateRoomType);
    }
}

