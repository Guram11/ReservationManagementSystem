using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.CreateRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.DeleteRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Commands.UpdateRoomType;
using ReservationManagementSystem.Application.Features.RoomTypes.Common;
using ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetAllRoomTypes;
using ReservationManagementSystem.Application.Features.RoomTypes.Queries.GetRoomTypeById;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomTypeResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams)
    {
        var response = await _mediator.Send(new GetAllRoomTypesRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize));
        return ResponseHandler.HandleResponse(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<ActionResult<RoomTypeResponse>> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetRoomTypeByIdRequest(id));
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<RoomTypeResponse>> Create(CreateRoomTypeRequest request)
    {
        var response = await _mediator.Send(request);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPut]
    public async Task<ActionResult<RoomTypeResponse>> Update(UpdateRoomTypeRequest request)
    {
        var response = await _mediator.Send(request);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<RoomTypeResponse>> Delete([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new DeleteRoomTypeRequest(id));
        return ResponseHandler.HandleResponse(response);
    }
}
