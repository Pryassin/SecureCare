using CMS.Application.Patients.Commands.CreatePatient;
using CMS.Application.Patients.Commands.UpdatePatient;
using CMS.Application.Patients.Queries.GetAllPatients;
using CMS.Application.Patients.Queries.GetPatientById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

public class PatientsController : ApiController
{
    public PatientsController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Admin,Receptionist,Doctor,Patient")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPatientByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = "Admin,Receptionist,Doctor")]
    [HttpGet]
    public async Task<IActionResult> GetAllPatients(CancellationToken cancellationToken)
    {
        var query = new GetAllPatientsQuery();
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = "Admin,Receptionist")]
    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetPatient), new { id = result.Value }, result.Value);
    }

    [Authorize(Roles = "Admin,Receptionist,Patient")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientCommand command, CancellationToken cancellationToken)
    {
        if (command.Id != id)
        {
            return BadRequest("The ID in the URL does not match the ID in the body."); 
        }

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
}
