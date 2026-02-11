using CMS.Application.Doctors.Queries.GetDoctorById;
using CMS.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers;

public class DoctorController : ApiController
{
    public DoctorController(ISender sender) : base(sender)
    {
        
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorById(Guid id, CancellationToken cancellationToken)
    {
         var query = new GetDoctorByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
}