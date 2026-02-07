using CMS.Domain.Common;

namespace CMS.Domain.Entities;

public class Prescription : BaseEntity
{
    public Guid AppointmentId { get; protected set; }
    public Appointment Appointment { get; protected set; } = null!;
    public string MedicationDetails { get; protected set; } = default!;
    public string? DoctorNotes { get; protected set; }
    public DateTimeOffset CreatedAt { get; protected set; }

    // Entity Framework Constructor
    protected Prescription() : base() { }

    private Prescription(Guid appointmentId, string medicationDetails, string? doctorNotes) 
        : base(Guid.Empty)
    {
        AppointmentId = appointmentId;
        MedicationDetails = medicationDetails;
        DoctorNotes = doctorNotes;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Result<Prescription> Create(Guid appointmentId, string medicationDetails, string? doctorNotes)
    {
        if (appointmentId == Guid.Empty)
            return Errors.Prescription.AppointmentIdRequired;

        if (string.IsNullOrWhiteSpace(medicationDetails))
            return Errors.Prescription.MedicationDetailsRequired;

        return new Prescription(appointmentId, medicationDetails, doctorNotes);
    }
}
