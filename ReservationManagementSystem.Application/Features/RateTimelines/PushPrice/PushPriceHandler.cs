using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Features.RateTimelines.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.RateTimelines.PushPrice;

public sealed class PushPriceHandler : IRequestHandler<PushPriceRequest, Result<RateTimelineResponse>>
{
    private readonly IRateTimelineRepository _rateTimelineRepository;
    private readonly IRateRoomTypeRepository _rateRoomTypeRepository;
    private readonly IMapper _mapper;

    public PushPriceHandler(IRateTimelineRepository rateTimelineRepository,
        IRateRoomTypeRepository rateRoomTypeRepository, IMapper mapper)
    {
        _rateTimelineRepository = rateTimelineRepository;
        _rateRoomTypeRepository = rateRoomTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<RateTimelineResponse>> Handle(PushPriceRequest request, CancellationToken cancellationToken)
    {
        var rateRoomType = await _rateRoomTypeRepository.GetRateRoomTypeWithRateTimelines(request.RateId, request.RoomTypeId, cancellationToken);

        if (rateRoomType is null)
        {
            return Result<RateTimelineResponse>.Failure(RateRoomTypeErrors.NotFound());
        }

        var rateTimeline = new RateTimeline();

        for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
        {
            rateTimeline = rateRoomType?.RateTimelines?
               .FirstOrDefault(at => at.Date == date);

            if (rateTimeline == null)
            {
                rateTimeline = new RateTimeline
                {
                    Date = date,
                    RateId = request.RateId,
                    RoomTypeId = request.RoomTypeId,
                    Price = request.Price
                };
                await _rateTimelineRepository.Create(rateTimeline, cancellationToken);
                rateRoomType?.RateTimelines?.Add(rateTimeline);
            }
            else
            {
                rateTimeline.Price = request.Price;
            }
        }

        await _rateRoomTypeRepository.SaveChangesAsync(cancellationToken);

        var respose = _mapper.Map<RateTimelineResponse>(rateTimeline);

        return Result<RateTimelineResponse>.Success(respose);
    }
}
