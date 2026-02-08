using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Persistence.Repositories;

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .Where(d => d.Specialization == specialization)
            .ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);
    }
}
