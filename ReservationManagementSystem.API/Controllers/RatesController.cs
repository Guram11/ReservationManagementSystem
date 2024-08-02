using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Features.Rates.Commands.CreateRate;
using ReservationManagementSystem.Application.Features.Rates.Commands.DeleteRate;
using ReservationManagementSystem.Application.Features.Rates.Commands.UpdateRate;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Features.Rates.Queries.GetAllRates;
using ReservationManagementSystem.Application.Features.Rates.Queries.GetRateById;
using ReservationManagementSystem.Domain.Settings;

namespace ReservationManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<RateResponse>>> GetAll([FromQuery] GetAllQueryParams queryParams)
        {
            var response = await _mediator.Send(new GetAllRatesRequest(
                queryParams.FilterOn, queryParams.FilterQuery,
                queryParams.SortBy, queryParams.IsAscending,
                queryParams.PageNumber, queryParams.PageSize));
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ActionResult<RateResponse>> Get([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetRateByIdRequest(id));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<RateResponse>> Create(CreateRateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<RateResponse>> Update(UpdateRateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<ActionResult<RateResponse>> Delete([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new DeleteRateRequest(id));
            return Ok(response);
        }
    }
}
