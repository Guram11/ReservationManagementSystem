using AutoMapper;
using MediatR;
using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;

public sealed class AuthenticateUserHandler : IRequestHandler<AuthenticateUserRequest, Result<AuthenticationResponse>>
{
    private readonly IAccountService _accountService;

    public AuthenticateUserHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result<AuthenticationResponse>> Handle(AuthenticateUserRequest request, CancellationToken cancellationToken)
    {
        var response = await _accountService.AuthenticateAsync(request);

        if (response.IsSuccess)
        {
            return Result<AuthenticationResponse>.Success(response.Data);
        }

        return Result<AuthenticationResponse>.Failure(response.Error);
    }
}