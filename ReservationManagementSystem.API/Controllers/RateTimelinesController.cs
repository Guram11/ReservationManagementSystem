using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.Common;
using ReservationManagementSystem.Application.Features.AvailabilityTimelines.PushAvailability;
using ReservationManagementSystem.Application.Features.RateTimelines.Common;
using ReservationManagementSystem.Application.Features.RateTimelines.PushPrice;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RateTimelinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RateTimelinesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<RateTimelineResponse>> PushPrice([FromBody] PushPriceRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
