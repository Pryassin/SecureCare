using CMS.Domain.Common;
namespace CMS.Domain.Entities;

public class Doctor : BaseEntity
{
    public Guid UserId { get; protected set; }
    public User User { get; protected set; } = null!;
    public string LicenseNumber { get; protected set; } = default!;
    public string Specialization { get; protected set; } = default!;
    
    public ICollection<Appointment> Appointments { get; protected set; } = new List<Appointment>();

    // Empty constructor for EF Core - must call base()
    protected Doctor() : base() { }

    // Constructor for new Doctors - passes Guid.Empty to let BaseEntity generate the ID
    private Doctor(Guid userId, string licenseNumber, string specialization) 
        : base(Guid.Empty) 
    {
        UserId = userId;
        LicenseNumber = licenseNumber;
        Specialization = specialization;
    }

    public static Result<Doctor> Create(Guid userId, string licenseNumber, string specialization)
    {
        if (userId == Guid.Empty)
        {
            return Errors.Doctor.UserIdRequired;
        }

        if (string.IsNullOrWhiteSpace(licenseNumber))
        {
            return Errors.Doctor.LicenseRequired;
        }

        if (string.IsNullOrWhiteSpace(specialization))
        {
            return Errors.Doctor.SpecializationRequired;
        }

        return new Doctor(userId, licenseNumber, specialization);
    }

    public Result Update(string licenseNumber, string specialization)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber))
        {
            return Errors.Doctor.LicenseRequired;
        }

        if (string.IsNullOrWhiteSpace(specialization))
        {
            return Errors.Doctor.SpecializationRequired;
        }

        LicenseNumber = licenseNumber;
        Specialization = specialization;

        return Result.Success; // Using the Success property we defined
    }
}