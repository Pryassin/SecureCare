using CMS.Application.Doctors.Commands.CreateDoctor;
using CMS.Application.Doctors.Commands.UpdateDoctor;
using CMS.Application.Doctors.Queries.GetDoctorById;
using CMS.Application.Doctors.Queries.GetAllDoctors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly ISender _sender;

    public DoctorsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorCommand command)
    {
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetDoctor), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] UpdateDoctorCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("The ID in the URL does not match the ID in the body.");
        }

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctor(Guid id)
    {
        var query = new GetDoctorByIdQuery(id);
        var result = await _sender.Send(query);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctors()
    {
        var result = await _sender.Send(new GetAllDoctorsQuery());

        if (result.IsFailure)
        {
             return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
