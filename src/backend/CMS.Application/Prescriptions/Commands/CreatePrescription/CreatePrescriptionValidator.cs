using FluentValidation;

namespace CMS.Application.Prescriptions.Commands.CreatePrescription;

public class CreatePrescriptionValidator : AbstractValidator<CreatePrescriptionCommand>
{
    public CreatePrescriptionValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty().WithMessage("Appointment ID is required.");
        
        RuleFor(x => x.MedicationDetails)
            .NotEmpty().WithMessage("Medication details are required.")
            .MaximumLength(1000).WithMessage("Medication details cannot exceed 1000 characters.");

        RuleFor(x => x.DoctorNotes)
            .MaximumLength(1000).WithMessage("Doctor notes cannot exceed 1000 characters.");
    }
}
