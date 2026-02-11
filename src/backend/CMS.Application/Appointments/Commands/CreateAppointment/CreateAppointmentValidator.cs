using FluentValidation;

namespace CMS.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty().WithMessage("Patient ID is required.");
        RuleFor(x => x.DoctorId).NotEmpty().WithMessage("Doctor ID is required.");
        
        RuleFor(x => x.DateTime)
            .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Appointment must be scheduled for a future date.");
    }
}
