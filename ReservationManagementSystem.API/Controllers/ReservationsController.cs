using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.Reservations.Commands.CreateReservation;
using ReservationManagementSystem.Application.Features.Reservations.Commands.DeleteReservation;
using ReservationManagementSystem.Application.Features.Reservations.Common;
using ReservationManagementSystem.Application.Features.Reservations.Queries.GetAllReservations;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservationResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams)
    {
        var response = await _mediator.Send(new GetAllReservationsRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> Create(CreateReservationRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<ReservationResponse>> Delete([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new DeleteReservationRequest(id));
        return Ok(response);
    }
}
