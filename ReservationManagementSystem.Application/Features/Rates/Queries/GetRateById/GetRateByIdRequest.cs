using MediatR;
using ReservationManagementSystem.Application.Features.Rates.Common;

namespace ReservationManagementSystem.Application.Features.Rates.Queries.GetRateById;

public sealed record GetRateByIdRequest(Guid Id) : IRequest<RateResponse>;
