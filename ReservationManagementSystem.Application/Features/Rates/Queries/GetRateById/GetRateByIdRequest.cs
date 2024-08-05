using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Queries.GetRateById;

public sealed record GetRateByIdRequest(Guid Id) : IRequest<Result<RateResponse>>;
