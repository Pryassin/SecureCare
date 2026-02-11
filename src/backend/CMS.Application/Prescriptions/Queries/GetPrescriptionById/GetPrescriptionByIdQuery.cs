using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Prescriptions.Queries.GetPrescriptionById;

public record GetPrescriptionByIdQuery(Guid Id) : IRequest<Result<PrescriptionResponse>>;

public record PrescriptionResponse(
    Guid Id,
    Guid AppointmentId,
    string MedicationDetails,
    string? DoctorNotes,
    DateTimeOffset CreatedAt);
