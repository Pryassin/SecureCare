using CMS.Domain.Entities;

namespace CMS.Domain.Repositories;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Patient?> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);
}
