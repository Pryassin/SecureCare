using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using MediatR;
using CMS.Domain.Common;

namespace CMS.Application.Patients.Queries.GetPatientById;

public record GetPatientByIdQuery(Guid Id) : IRequest<Result<PatientResponse>>;

public record PatientResponse(
    Guid Id,
    Guid UserId,
    string NationalId,
    DateTime DateOfBirth,
    string Phone,
    string? EmergencyContact);

public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientResponse>>
{
    private readonly IPatientRepository _patientRepository;

    public GetPatientByIdQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<PatientResponse>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (patient is null)
        {
            return (Result<PatientResponse>)Result.Failure(Errors.User.NotFound); // Or generic NotFound error
        }

        var response = new PatientResponse(
            patient.Id,
            patient.UserId,
            patient.NationalId,
            patient.DateOfBirth,
            patient.Phone,
            patient.EmergencyContact);

        return response;
    }
}
