using CMS.Domain.Entities;
using CMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CMS.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Ensure database is created (or migrations applied)
        // context.Database.Migrate(); // Optional: enable if you want auto-migration on start

        if (context.Users.Any())
        {
            return; // DB has been seeded
        }

        // 1. Create Users
        var adminUser = User.Create("admin@securecare.com", "Password123!", UserRole.Admin).Value;
        var doctorUser = User.Create("dr.smith@securecare.com", "Password123!", UserRole.Doctor).Value;
        var patientUser = User.Create("john.doe@email.com", "Password123!", UserRole.Patient).Value;
        var receptionistUser = User.Create("reception@securecare.com", "Password123!", UserRole.Receptionist).Value;

        context.Users.AddRange(adminUser, doctorUser, patientUser, receptionistUser);
        context.SaveChanges();

        // 2. Create Doctor Profile
        var doctor = Doctor.Create(doctorUser.Id, "LIC-12345", "Cardiology").Value;
        context.Doctors.Add(doctor);
        context.SaveChanges();

        // 3. Create Patient Profile
        var patient = Patient.Create(
            patientUser.Id,
            "1234567890",
            new DateTime(1985, 5, 20, 0, 0, 0, DateTimeKind.Utc),
            Gender.Male,
            "+1-555-0101",
            "Jane Doe (+1-555-0102)"
        ).Value;
        context.Patients.Add(patient);
        context.SaveChanges();

        // 4. Create Appointment
        var appointmentDate = DateTimeOffset.UtcNow.AddDays(2).Date.AddHours(14); // 2 days from now at 2 PM
        var appointment = Appointment.Create(patient.Id, doctor.Id, appointmentDate).Value;
        context.Appointments.Add(appointment);
        context.SaveChanges();
    }
}
