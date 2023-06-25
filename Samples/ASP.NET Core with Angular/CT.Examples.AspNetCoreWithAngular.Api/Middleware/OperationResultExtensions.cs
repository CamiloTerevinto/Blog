using CT.Examples.AspNetCoreWithAngular.Models;
using Microsoft.AspNetCore.Mvc;

namespace CT.Examples.AspNetCoreWithAngular.Api.Middleware;

internal static class OperationResultExtensions
{
    internal static async Task<ActionResult<T>> ToActionResult<T>(this Task<OperationResult<T>> operation)
    {
        var result = await operation;

        return result.Status switch
        {
            OperationResultStatus.Success => new OkObjectResult(result.Data),
            OperationResultStatus.BadRequest => BadRequest("Bad request", result.ErrorMessage),
            OperationResultStatus.Forbidden => new ForbidResult(),
            _ => new ObjectResult("Internal server error") { StatusCode = StatusCodes.Status500InternalServerError },
        };
    }

    internal static async Task<IActionResult> ToActionResult(this Task<OperationResultStatus> operation)
    {
        var status = await operation;

        return status switch
        {
            OperationResultStatus.Success => new NoContentResult(),
            OperationResultStatus.BadRequest => BadRequest("Bad request", null),
            OperationResultStatus.Forbidden => new ForbidResult(),
            _ => new ObjectResult("Internal server error") { StatusCode = StatusCodes.Status500InternalServerError },
        };
    }

    private static BadRequestObjectResult BadRequest(string title, string? detail)
    {
        return new BadRequestObjectResult(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = title,
            Detail = detail
        });
    }
}