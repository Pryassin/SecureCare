using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Persistence.Repositories;

public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Prescription>> GetByAppointmentIdAsync(Guid appointmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Prescriptions
            .Where(p => p.AppointmentId == appointmentId)
            .ToListAsync(cancellationToken);
    }
}
