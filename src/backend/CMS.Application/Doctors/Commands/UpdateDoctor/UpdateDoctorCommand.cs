using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Doctors.Commands.UpdateDoctor;

public record UpdateDoctorCommand(
    Guid Id,
    string LicenseNumber,
    string Specialization) : IRequest<Result>;
