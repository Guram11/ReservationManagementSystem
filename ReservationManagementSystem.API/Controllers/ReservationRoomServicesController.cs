using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Commands.CreateReservationRoomService;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Commands.DeleteReservationRoomService;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Common;
using ReservationManagementSystem.Application.Features.ReservationRoomService.Queries.GetAllReservationRoomServices;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationRoomServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationRoomServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservationRoomServiceResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllReservationRoomServicesRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationRoomServiceResponse>> Create(CreateReservationRoomServiceRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpDelete]
    [Route("{reservationRoomId:guid}/{hotelServiceId:guid}")]
    public async Task<ActionResult<ReservationRoomServiceResponse>> Delete([FromRoute] Guid reservationRoomId, [FromRoute] Guid hotelServiceId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteReservationRoomServiceRequest(reservationRoomId, hotelServiceId), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }
}
