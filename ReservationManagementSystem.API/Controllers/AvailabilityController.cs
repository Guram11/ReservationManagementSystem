﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;
using ReservationManagementSystem.Application.Features.AvailibilityTimeline.PushAvailability;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvailabilityController : ControllerBase
{
    private readonly IMediator _mediator;

    public AvailabilityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<AvailabilityResponse>> PushAvailability([FromBody] PushAvailabilityRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<List<CheckAvailabilityResponse>>> CheckAvailability([FromQuery] CheckAvailabilityRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
