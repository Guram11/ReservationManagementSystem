using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.API.Extensions;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.CreateHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Commands.DeleteHotelService;
using ReservationManagementSystem.Application.Features.HotelServices.Common;
using ReservationManagementSystem.Application.Features.HotelServices.Queries;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "SuperAdmin, Basic")]
public class HotelServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<HotelServiceResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllHotelServicesRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }

    [HttpPost]
    public async Task<ActionResult<HotelServiceResponse>> Create(CreateHotelServiceRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }


    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<HotelServiceResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteHotelServiceRequest(id), cancellationToken);
        return ResponseHandler.HandleResponse(response);
    }
}
