﻿using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.Common.Errors;
using ReservationManagementSystem.Application.Features.Hotels.Common;
using ReservationManagementSystem.Application.Features.Rates.Common;
using ReservationManagementSystem.Application.Interfaces.Repositories;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Rates.Commands.DeleteRate;

public sealed class DeleteRateHandler : IRequestHandler<DeleteRateRequest, Result<RateResponse>>
{
    private readonly IRateRepository _rateRepository;
    private readonly IMapper _mapper;

    public DeleteRateHandler(IRateRepository rateRepository, IMapper mapper)
    {
        _rateRepository = rateRepository;
        _mapper = mapper;
    }

    public async Task<Result<RateResponse>> Handle(DeleteRateRequest request, CancellationToken cancellationToken)
    {
        var isInUse = await _rateRepository.IsRateInUseAsync(request.Id);
        if (isInUse)
        {
            return Result<RateResponse>.Failure(ValidationError.ResourceInUse());
        }

        var rate = await _rateRepository.Delete(request.Id, cancellationToken);
        
        if (rate == null)
        {
            return Result<RateResponse>.Failure(RateErrors.NotFound(request.Id));
        }
        
        var response = _mapper.Map<RateResponse>(rate);
        return Result<RateResponse>.Success(response);
    }
}
