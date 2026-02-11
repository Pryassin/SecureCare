using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Doctors.Commands.CreateDoctor;

public record CreateDoctorCommand(
    Guid UserId,
    string LicenseNumber,
    string Specialization) : IRequest<Result<Guid>>;
