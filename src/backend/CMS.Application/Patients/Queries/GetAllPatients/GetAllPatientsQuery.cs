using CMS.Application.Patients.Queries.GetPatientById;
using CMS.Domain.Common;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Patients.Queries.GetAllPatients;

public record GetAllPatientsQuery() : IRequest<Result<List<PatientResponse>>>;

public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, Result<List<PatientResponse>>>
{
    private readonly IPatientRepository _patientRepository;

    public GetAllPatientsQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<List<PatientResponse>>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var patients = await _patientRepository.GetAllAsync(cancellationToken);
        
        var response = patients.Select(p => new PatientResponse(
            p.Id,
            p.UserId,
            p.NationalId,
            p.DateOfBirth,
            p.Phone,
            p.EmergencyContact)).ToList();

        return response;
    }
}
