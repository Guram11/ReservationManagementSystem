using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.Rooms.Commands.CreateRoom;
using ReservationManagementSystem.Application.Features.Rooms.Commands.DeleteRoom;
using ReservationManagementSystem.Application.Features.Rooms.Commands.UpdateRoom;
using ReservationManagementSystem.Application.Features.Rooms.Common;
using ReservationManagementSystem.Application.Features.Rooms.Queries.GetAllRooms;
using ReservationManagementSystem.Application.Features.Rooms.Queries.GetRoomById;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams)
    {
        var response = await _mediator.Send(new GetAllRoomsRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize));
        return ResponseHandler.HandleResponse(response);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<ActionResult<RoomResponse>> Get([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new GetRoomByIdRequest(id));
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<RoomResponse>> Create(CreateRoomRequest request)
    {
        var response = await _mediator.Send(request);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPut]
    public async Task<ActionResult<RoomResponse>> Update(UpdateRoomRequest request)
    {
        var response = await _mediator.Send(request);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<RoomResponse>> Delete([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new DeleteRoomRequest(id));
        return ResponseHandler.HandleResponse(response);
    }
}
