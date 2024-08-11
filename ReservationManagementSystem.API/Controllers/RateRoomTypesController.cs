using Microsoft.AspNetCore.Mvc;
using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Queries.GetAllRateRoomTypes;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RateRoomTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RateRoomTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<RateRoomTypeResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams)
    {
        var response = await _mediator.Send(new GetAllRateRoomTypesRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<RateRoomTypeResponse>> Create(CreateRateRoomTypeRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{rateId:guid}/{roomTypeId:guid}")]
    public async Task<ActionResult<RateRoomTypeResponse>> Delete([FromRoute] Guid rateId, [FromRoute] Guid roomTypeId)
    {
        var response = await _mediator.Send(new DeleteRateRoomTypeRequest(rateId, roomTypeId));
        return Ok(response);
    }
}   
