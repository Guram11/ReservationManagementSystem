﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.Hotels.Commands.CreateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Commands.DeleteHotel;
using ReservationManagementSystem.Application.Features.Hotels.Commands.UpdateHotel;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Hotels.Queries.GetAllHotels;
using ReservationManagementSystem.Application.Features.Hotels.Queries.GetHotelById;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "SuperAdmin, Basic")]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<HotelResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllHotelsRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<ActionResult<HotelResponse>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetHotelByIdRequest(id), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<HotelResponse>> Create(CreateHotelRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPut]
    public async Task<ActionResult<HotelResponse>> Update(UpdateHotelRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<HotelResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteHotelRequest(id), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }
}
