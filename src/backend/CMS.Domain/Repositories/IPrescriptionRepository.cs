using CMS.Domain.Entities;

namespace CMS.Domain.Repositories;

public interface IPrescriptionRepository : IRepository<Prescription>
{
    Task<List<Prescription>> GetByAppointmentIdAsync(Guid appointmentId, CancellationToken cancellationToken = default);
}
