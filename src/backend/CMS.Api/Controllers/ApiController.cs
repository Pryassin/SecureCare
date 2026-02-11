using CMS.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender _sender;

    protected ApiController(ISender sender)
    {
        _sender = sender;
    }

    protected IActionResult HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ =>
                Problem(
                    statusCode: GetStatusCode(result.Error),
                    title: GetTitle(result.Error),
                    detail: result.Error.Description,
                    extensions: new Dictionary<string, object?>
                    {
                        { "errors", new[] { result.Error } }
                    })
        };
    }

    private static int GetStatusCode(Error error) =>
        error.Code switch
        {
            "User.NotFound" => StatusCodes.Status404NotFound,
            "Patient.NotFound" => StatusCodes.Status404NotFound,
            "Doctor.NotFound" => StatusCodes.Status404NotFound,
            "Appointment.NotFound" => StatusCodes.Status404NotFound,
            "Prescription.NotFound" => StatusCodes.Status404NotFound,
            "Appointment.Conflict" => StatusCodes.Status409Conflict,
            "Auth.InvalidCredentials" => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest
        };

    private static string GetTitle(Error error) =>
        error.Code switch
        {
            "User.NotFound" => "Not Found",
            "Patient.NotFound" => "Not Found",
            "Doctor.NotFound" => "Not Found",
            "Appointment.NotFound" => "Not Found",
            "Prescription.NotFound" => "Not Found",
            "Appointment.Conflict" => "Conflict",
            "Auth.InvalidCredentials" => "Unauthorized",
            _ => "Bad Request"
        };
}
