using CMS.Domain.Common;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Doctors.Queries.GetDoctorById;

public record GetDoctorByIdQuery(Guid Id) : IRequest<Result<DoctorResponse>>;

public record DoctorResponse(
    Guid Id,
    Guid UserId,
    string LicenseNumber,
    string Specialization);

public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Result<DoctorResponse>>
{
    private readonly IDoctorRepository _doctorRepository;

    public GetDoctorByIdQueryHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<Result<DoctorResponse>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (doctor is null)
        {
            return (Result<DoctorResponse>)Result.Failure(Errors.User.NotFound);
        }

        var response = new DoctorResponse(
            doctor.Id,
            doctor.UserId,
            doctor.LicenseNumber,
            doctor.Specialization);

        return response;
    }
}
