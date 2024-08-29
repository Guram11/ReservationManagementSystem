using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Enums;
using ReservationManagementSystem.Application.Wrappers;

namespace ReservationManagementSystem.API.Extensions;

public static class ResponseHandler
{
    public static ActionResult HandleResponse<T>(Result<T> response)
    {
        if (response.IsSuccess)
        {
            return new OkObjectResult(response.Data);
        }

        return response.Error.ErrorType switch
        {
            ErrorType.ValidationError => new BadRequestObjectResult(response.Error.Description),
            ErrorType.NoAvailableOptionsError => new BadRequestObjectResult(response.Error.Description),
            ErrorType.InvalidDataPassedError => new BadRequestObjectResult(response.Error.Description),
            ErrorType.AlreadyCreatedError => new BadRequestObjectResult(response.Error.Description),
            ErrorType.ExceedingNumberOfRooms => new BadRequestObjectResult(response.Error.Description),
            ErrorType.EmailNotSentError => new BadRequestObjectResult(response.Error.Description),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(response.Error.Description),
            ErrorType.Forbidden => new BadRequestObjectResult(response.Error.Description),
            ErrorType.InvalidCredentials => new BadRequestObjectResult(response.Error.Description),
            ErrorType.NotFoundError => new NotFoundObjectResult(response.Error.Description),
            _ => new StatusCodeResult(500)
        };
    }
}
