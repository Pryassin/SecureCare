using CMS.Domain.Entities;

namespace CMS.Domain.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<List<Appointment>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);
}
