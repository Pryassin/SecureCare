namespace CMS.Domain.Entities;

public partial class Appointment
{
    public class Prescription : BaseEntity
{
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;
    public string? MedicationDetails { get; set; }
    public string? DoctorNotes { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
}


