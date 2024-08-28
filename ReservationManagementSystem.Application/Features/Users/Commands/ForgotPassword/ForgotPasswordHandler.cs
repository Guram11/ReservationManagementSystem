using MediatR;
using ReservationManagementSystem.Application.Features.Users.Commands.Common;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Users.Commands.ForgotPassword;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequest, Result<string>>
{
    private readonly IAccountService _accountService;

    public ForgotPasswordHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result<string>> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var response = await _accountService.ForgotPassword(request);

        if (response.IsSuccess)
        {
            return Result<string>.Success(SuccessResponses.ResetTokenSent);
        }

        return Result<string>.Failure(response.Error);
    }
}
