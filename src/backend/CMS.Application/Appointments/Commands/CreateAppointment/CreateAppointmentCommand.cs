using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    Guid PatientId,
    Guid DoctorId,
    DateTimeOffset DateTime) : IRequest<Result<Guid>>;
