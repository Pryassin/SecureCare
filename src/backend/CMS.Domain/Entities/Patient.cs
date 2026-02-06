namespace CMS.Domain.Entities;

public class Patient : BaseEntity
{
    public int? UserId { get; set; }
    public User? User { get; set; }
    public string? NationalId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? Phone { get; set; }
    public string? EmergencyContact { get; set; }
    
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}


