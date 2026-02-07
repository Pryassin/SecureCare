using CMS.Domain.Common;
using CMS.Domain.Enums;

namespace CMS.Domain.Entities;

public class Patient : BaseEntity
{
    public Guid UserId { get; protected set; }
    public User User { get; protected set; } = null!;
    public string NationalId { get; protected set; } = default!;
    public DateTime DateOfBirth { get; protected set; }
    public Gender Gender { get; protected set; }
    public string Phone { get; protected set; } = default!;
    public string? EmergencyContact { get; protected set; }
    
    public ICollection<Appointment> Appointments { get; protected set; } = new List<Appointment>();

    // Entity Framework Constructor
    protected Patient() : base() { }

    private Patient(Guid userId, string nationalId, DateTime dateOfBirth, Gender gender, string phone, string? emergencyContact) 
        : base(Guid.Empty)
    {
        UserId = userId;
        NationalId = nationalId;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Phone = phone;
        EmergencyContact = emergencyContact;
    }

    public static Result<Patient> Create(Guid userId, string nationalId, DateTime dateOfBirth, Gender gender, string phone, string? emergencyContact)
    {
        if (userId == Guid.Empty)
            return Errors.Patient.UserIdRequired;

        if (string.IsNullOrWhiteSpace(nationalId))
            return Errors.Patient.NationalIdRequired;

        if (string.IsNullOrWhiteSpace(phone))
            return Errors.Patient.PhoneRequired;

        return new Patient(userId, nationalId, dateOfBirth, gender, phone, emergencyContact);
    }

    public Result Update(string nationalId, string phone, string? emergencyContact)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
            return Errors.Patient.NationalIdRequired;

        if (string.IsNullOrWhiteSpace(phone))
            return Errors.Patient.PhoneRequired;

        NationalId = nationalId;
        Phone = phone;
        EmergencyContact = emergencyContact;

        return Result.Success;
    }
}
