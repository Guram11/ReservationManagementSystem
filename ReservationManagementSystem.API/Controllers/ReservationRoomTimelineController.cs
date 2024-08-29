using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.ReservationRoomTimelines.Common;
using ReservationManagementSystem.Application.Features.ReservationRoomTimelines.PushReservationTimeline;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationRoomTimelineController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationRoomTimelineController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ReservationRoomTimelineResponse>> PushReservationRoomTimeline([FromBody] PushReservationTimelineRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }
}
