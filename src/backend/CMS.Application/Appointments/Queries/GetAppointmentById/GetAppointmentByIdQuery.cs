using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Appointments.Queries.GetAppointmentById;

public record GetAppointmentByIdQuery(Guid Id) : IRequest<Result<AppointmentResponse>>;

public record AppointmentResponse(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    DateTimeOffset DateTime,
    string Status);
