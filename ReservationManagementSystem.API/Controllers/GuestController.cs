using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.GuestFeatures.Common;
using ReservationManagementSystem.Application.Features.GuestFeatures.CreateGuest;
using ReservationManagementSystem.Application.Features.GuestFeatures.DeleteGuest;
using ReservationManagementSystem.Application.Features.GuestFeatures.GetAllGuests;
using ReservationManagementSystem.Application.Features.GuestFeatures.GetGuestById;
using ReservationManagementSystem.Application.Features.GuestFeatures.UpdateGuest;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GuestController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<GuestResponse>>> GetAll(
        [FromQuery] string? filterOn, [FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool isAscending = true,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1)
    {
        var response = await _mediator.Send(new GetAllUserRequest(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize));
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<ActionResult<GuestResponse>> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetGuestByIdRequest(id));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<GuestResponse>> Create(CreateGuestRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<GuestResponse>> Update(UpdateGuestRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<GuestResponse>> Delete([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new DeleteGuestRequest(id));
        return Ok(response);
    }
}
