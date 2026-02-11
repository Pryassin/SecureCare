using CMS.Application.Doctors.Queries.GetDoctorById;
using CMS.Domain.Common;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Doctors.Queries.GetAllDoctors;

public record GetAllDoctorsQuery() : IRequest<Result<List<DoctorResponse>>>;

public class GetAllDoctorsQueryHandler : IRequestHandler<GetAllDoctorsQuery, Result<List<DoctorResponse>>>
{
    private readonly IDoctorRepository _doctorRepository;

    public GetAllDoctorsQueryHandler(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<Result<List<DoctorResponse>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _doctorRepository.GetAllAsync(cancellationToken);
        
        var response = doctors.Select(d => new DoctorResponse(
            d.Id,
            d.UserId,
            d.LicenseNumber,
            d.Specialization)).ToList();

        return response;
    }
}
