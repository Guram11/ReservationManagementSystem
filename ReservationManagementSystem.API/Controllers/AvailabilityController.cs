using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.PushAvailability;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AvailabilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("push-availability")]
        public async Task<ActionResult<Result<string>>> PushAvailability([FromBody] PushAvailabilityRequest request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
