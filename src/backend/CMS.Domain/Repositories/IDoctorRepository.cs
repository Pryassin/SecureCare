using CMS.Domain.Entities;

namespace CMS.Domain.Repositories;

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<Doctor?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);
}
