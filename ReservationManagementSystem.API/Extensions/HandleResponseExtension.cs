using Microsoft.AspNetCore.Mvc;
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

        return response.Error.Code switch
        {
            "ValidationError" => new BadRequestObjectResult(response.Error.Description),
            "NotFound" => new NotFoundObjectResult(response.Error.Description),
            _ => new StatusCodeResult(500)
        };
    }
}
