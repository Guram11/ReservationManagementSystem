using MediatR;
using ReservationManagementSystem.Application.Interfaces.Services;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.Application.Features.Users.Commands.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, Result<string>>
{
    private readonly IAccountService _accountService;

    public ResetPasswordHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<Result<string>> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var response = await _accountService.ResetPassword(request);

        if (response.IsSuccess)
        {
            return Result<string>.Success("Password reset successfully!");
        }

        return Result<string>.Failure(response.Error);
    }
}
