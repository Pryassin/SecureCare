using CMS.Domain.Entities;
using CMS.Domain.Enums;
using CMS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infrastructure.Persistence.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {

    }

    public async Task<List<Appointment>> GetByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
                      .Where(a => a.DateTime >= startDate && a.DateTime <= endDate)
                      .ToListAsync(cancellationToken);
    }

    public async Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.Where(a => a.DoctorId == doctorId).ToListAsync(cancellationToken);
    }

    public async Task<List<Appointment>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.Where(a => a.PatientId == patientId).ToListAsync(cancellationToken);
    }

    public async Task<bool> IsDoctorAvailableAsync(Guid doctorId, DateTimeOffset dateTime, CancellationToken cancellationToken = default)
    {
        // Simple 30-minute slot check logic for demonstration.
        // In a real app, you might have variable durations.
        var endTime = dateTime.AddMinutes(30);

        return !await _context.Appointments
            .AnyAsync(a => 
                a.DoctorId == doctorId &&
                a.Status != AppointmentStatus.Cancelled &&
                a.DateTime < endTime && 
                a.DateTime.AddMinutes(30) > dateTime,
                cancellationToken);
    }
}