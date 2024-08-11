using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.Common;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.PushAvailability;

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
}
