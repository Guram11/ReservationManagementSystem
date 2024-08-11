using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;
using ReservationManagementSystem.Domain.Entities;

namespace ReservationManagementSystem.Application.Features.Rates.Queries.GetRateById;

public sealed class GetRateByIdHandler : IRequestHandler<GetRateByIdRequest, Result<RateResponse>>
{
    private readonly IRateRepository _rateRepository;
    private readonly IMapper _mapper;

    public GetRateByIdHandler(IRateRepository rateRepository, IMapper mapper)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
    }

    public async Task<Result<RateResponse>> Handle(GetRateByIdRequest request, CancellationToken cancellationToken)
    {
        var rate = await _rateRepository.Get(request.Id);

        if (rate is null)
        {
            return Result<RateResponse>.Failure(NotFoundError.NotFound($"Rate with ID {request.Id} was not found."));
        }

        var response = _mapper.Map<RateResponse>(rate);
        return Result<RateResponse>.Success(response);
    }
}
