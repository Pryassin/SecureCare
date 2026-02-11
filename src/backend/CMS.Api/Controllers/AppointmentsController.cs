using CMS.Application.Appointments.Commands.CancelAppointment;
using CMS.Application.Appointments.Commands.CreateAppointment;
using CMS.Application.Appointments.Queries.GetAppointmentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

public class AppointmentsController : ApiController
{
    public AppointmentsController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAppointment(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAppointmentByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetAppointment), new { id = result.Value }, result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelAppointment(Guid id, CancellationToken cancellationToken)
    {
        var command = new CancelAppointmentCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
}
