﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.ReservationRooms.Commands.CreateReservationRoom;
using ReservationManagementSystem.Application.Features.ReservationRooms.Commands.DeleteReservationRoom;
using ReservationManagementSystem.Application.Features.ReservationRooms.Common;
using ReservationManagementSystem.Application.Features.ReservationRooms.Queries;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationRoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationRoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservationRoomResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllReservationRoomsRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationRoomResponse>> Create(CreateReservationRoomRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{rateId:guid}/{roomTypeId:guid}")]
    public async Task<ActionResult<ReservationRoomResponse>> Delete([FromRoute] Guid reservationId, [FromRoute] Guid roomId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteReservationRoomRequest(reservationId, roomId), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }
}
