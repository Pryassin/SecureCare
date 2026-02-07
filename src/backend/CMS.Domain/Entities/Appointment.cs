using CMS.Domain.Common;
using CMS.Domain.Enums;

namespace CMS.Domain.Entities;

public partial class Appointment : BaseEntity
{
    public Guid PatientId { get; protected set; }
    public Patient Patient { get; protected set; } = null!;
    
    // Changed to Guid to match Doctor.Id
    public Guid DoctorId { get; protected set; }
    public Doctor Doctor { get; protected set; } = null!;
    
    public DateTimeOffset DateTime { get; protected set; }
    public AppointmentStatus Status { get; protected set; }
    
    public ICollection<Prescription> Prescriptions { get; protected set; } = new List<Prescription>();

    // Required for EF Core
    protected Appointment() : base() { }

    private Appointment(Guid ID,Guid patientId, Guid doctorId, DateTimeOffset dateTime) 
        : base(ID)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        DateTime = dateTime;
        Status = AppointmentStatus.Confirmed; // Default status
    }

    public static Result<Appointment> Create(Guid patientId, Guid doctorId, DateTimeOffset dateTime)
    {
        if (patientId == Guid.Empty)
            return Errors.Appointment.PatientIdRequired;

        if (doctorId == Guid.Empty)
            return Errors.Appointment.DoctorIdRequired;

        if (dateTime < DateTimeOffset.UtcNow)
            return Errors.Appointment.PastDate;

        return new Appointment(Guid.NewGuid(),patientId, doctorId, dateTime);
    }

    public Result UpdateStatus(AppointmentStatus newStatus)
    {
        // Business logic: e.g., you can't cancel a completed appointment
        Status = newStatus;
        return Result.Success;
    }

    public Result Reschedule(DateTimeOffset newDateTime)
    {
        if (newDateTime < DateTimeOffset.UtcNow)
            return Errors.Appointment.PastDate;

        DateTime = newDateTime;
        return Result.Success;
    }
}