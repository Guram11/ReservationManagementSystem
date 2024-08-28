using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;

public sealed class CreateRateRoomTypeHandler : IRequestHandler<CreateRateRoomTypeRequest, Result<RateRoomTypeResponse>>
{
    private readonly IRateRoomTypeRepository _rateRoomTypeRepository;
    private readonly IMapper _mapper;

    public CreateRateRoomTypeHandler(IRateRoomTypeRepository rateRoomTypeRepository, IMapper mapper)
    {
        _rateRoomTypeRepository = rateRoomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<RateRoomTypeResponse>> Handle(CreateRateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var rateRoomType = _mapper.Map<RateRoomType>(request);
        var createdRateRoomType = await _rateRoomTypeRepository.Create(rateRoomType);

        if (createdRateRoomType == null)
        {
            return Result<RateRoomTypeResponse>.Failure(RateRoomTypeErrors.NotFound());
        }

        var response = _mapper.Map<RateRoomTypeResponse>(rateRoomType);
        return Result<RateRoomTypeResponse>.Success(response);
    }
}
