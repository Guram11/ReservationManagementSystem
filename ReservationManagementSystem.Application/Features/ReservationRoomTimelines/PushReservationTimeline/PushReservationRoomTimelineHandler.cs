﻿using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Features.ReservationRoomTimelines.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.ReservationRoomTimelines.PushReservationTimeline;

public sealed class PushReservationRoomTimeline : IRequestHandler<PushReservationTimelineRequest, Result<ReservationRoomTimelineResponse>>
{
    private readonly IReservationRoomTimelineRepository _reservationRoomTimelineRepository;
    private readonly IReservationRoomRepository _reservationRoomRepository;
    private readonly IMapper _mapper;

    public PushReservationRoomTimeline(IReservationRoomTimelineRepository reservationRoomTimelineRepository,
        IReservationRoomRepository reservationRoomRepository ,IMapper mapper)
    {
        _reservationRoomTimelineRepository = reservationRoomTimelineRepository;
        _reservationRoomRepository = reservationRoomRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReservationRoomTimelineResponse>> Handle(PushReservationTimelineRequest request, CancellationToken cancellationToken)
    {
        var reservationRoom = await _reservationRoomRepository.GetReservationRoomWithTimeline(request.ReservationRoomId, cancellationToken);

        if (reservationRoom is null)
        {
            return Result<ReservationRoomTimelineResponse>.Failure(ReservationRoomErrors.NotFound());
        }

        var lastModifiedRateTimeline = new ReservationRoomTimeline();

        for (var date = request.StartDate; date <= request.EndDate; date = date.AddDays(1))
        {
            var reservationRoomTimeline = reservationRoom.ReservationRoomTimelines!
                .FirstOrDefault(rt => rt.Date == date);

            if (reservationRoomTimeline == null)
            {
                reservationRoomTimeline = new ReservationRoomTimeline
                {
                    Date = date,
                    ReservationRoomId = reservationRoom.Id,
                    Price = request.Price
                };
                await _reservationRoomTimelineRepository.Create(reservationRoomTimeline, cancellationToken);
                reservationRoom.ReservationRoomTimelines!.Add(reservationRoomTimeline);
            }
            else
            {
                reservationRoomTimeline.Price = request.Price;
            }

            lastModifiedRateTimeline = reservationRoomTimeline;
        }

        await _reservationRoomRepository.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<ReservationRoomTimelineResponse>(lastModifiedRateTimeline);

        return Result<ReservationRoomTimelineResponse>.Success(response);
    }
}
