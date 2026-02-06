using CMS.Domain.Enums;

namespace CMS.Domain.Entities;

public partial class Appointment : BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    public DateTimeOffset DateTime { get; set; }
    public AppointmentStatus Status { get; set; }
    
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}


