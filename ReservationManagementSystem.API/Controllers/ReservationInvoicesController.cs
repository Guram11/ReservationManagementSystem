using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.CreateReservationInvoice;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Commands.DeleteReservationInvoice;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;
using ReservationManagementSystem.Application.Features.ResrevationInvoices.Queries;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationInvoicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationInvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservationInvoiceResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams)
    {
        var response = await _mediator.Send(new GetAllReservationInvoicesRequest(
            queryParams.FilterOn, queryParams.FilterQuery,
            queryParams.SortBy, queryParams.IsAscending,
            queryParams.PageNumber, queryParams.PageSize));
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationInvoiceResponse>> Create(CreateReservationInvoiceRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<ActionResult<ReservationInvoiceResponse>> Delete([FromRoute] Guid id)
    {
        var response = await _mediator.Send(new DeleteReservationInvoiceRequest(id));
        return Ok(response);
    }
}
