﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.Guests.Commands.DeleteGuest;
using ReservationManagementSystem.Application.Features.Guests.Queries.GetGuestById;
using ReservationManagementSystem.Application.Features.Guests.Commands.CreateGuest;
using ReservationManagementSystem.Application.Features.Guests.Commands.UpdateGuest;
using ReservationManagementSystem.Application.Features.Guests.Common;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Application.Features.Guests.Queries.GetAllGuests;
using ReservationManagementSystem.API.Extensions;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GuestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<GuestResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams)
    {
        var response = await _mediator.Send(new GetAllGuestsRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize));
        return ResponseHandler.HandleResponse(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<ActionResult<GuestResponse>> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetGuestByIdRequest(id));
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<GuestResponse>> Create(CreateGuestRequest request)
    {
        var response = await _mediator.Send(request);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPut]
    public async Task<ActionResult<GuestResponse>> Update(UpdateGuestRequest request)
    {
        var response = await _mediator.Send(request);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<GuestResponse>> Delete([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new DeleteGuestRequest(id));
        return ResponseHandler.HandleResponse(response);
    }
}
