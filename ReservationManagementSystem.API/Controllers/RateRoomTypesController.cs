using Microsoft.AspNetCore.Mvc;
using MediatR;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Common;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.CreateRateRoomType;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Commands.DeleteRateRoomType;
using ReservationManagementSystem.Domain.Settings;
using ReservationManagementSystem.Application.Features.RateRoomTypes.Queries.GetAllRateRoomTypes;
using ReservationManagementSystem.API.Extensions;

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
    public async Task<ActionResult<List<RateRoomTypeResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllRateRoomTypesRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<RateRoomTypeResponse>> Create(CreateRateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{rateId:guid}/{roomTypeId:guid}")]
    public async Task<ActionResult<RateRoomTypeResponse>> Delete([FromRoute] Guid rateId, [FromRoute] Guid roomTypeId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteRateRoomTypeRequest(rateId, roomTypeId), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }
}   
