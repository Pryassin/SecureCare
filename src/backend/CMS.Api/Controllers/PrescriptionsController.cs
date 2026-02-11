using CMS.Application.Prescriptions.Commands.CreatePrescription;
using CMS.Application.Prescriptions.Queries.GetPrescriptionById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

public class PrescriptionsController : ApiController
{
    public PrescriptionsController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescription(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPrescriptionByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] CreatePrescriptionCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(nameof(GetPrescription), new { id = result.Value }, result.Value);
    }
}
