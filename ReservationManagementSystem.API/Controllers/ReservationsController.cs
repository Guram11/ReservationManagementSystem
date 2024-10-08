﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Application.Features.Reservations.Commands.DeleteReservation;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Features.Reservations.Queries.GetAllReservations;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin, Basic")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservationResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllReservationsRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> Create(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<ReservationResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteReservationRequest(id), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }
}
