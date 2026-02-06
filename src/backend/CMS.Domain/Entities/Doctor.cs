namespace CMS.Domain.Entities;
public class Doctor : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}





