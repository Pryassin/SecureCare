using CMS.Application.Doctors.Commands.CreateDoctor;
using CMS.Application.Doctors.Commands.UpdateDoctor;
using CMS.Application.Doctors.Queries.GetAllDoctors;
using CMS.Application.Doctors.Queries.GetDoctorById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

public class DoctorsController : ApiController
{
    public DoctorsController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctor(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetDoctorByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctors(CancellationToken cancellationToken)
    {
        var query = new GetAllDoctorsQuery();
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetDoctor), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] UpdateDoctorCommand command, CancellationToken cancellationToken)
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
