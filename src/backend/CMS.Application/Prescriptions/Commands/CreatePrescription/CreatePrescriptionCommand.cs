using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Prescriptions.Commands.CreatePrescription;

public record CreatePrescriptionCommand(
    Guid AppointmentId,
    string MedicationDetails,
    string? DoctorNotes) : IRequest<Result<Guid>>;
