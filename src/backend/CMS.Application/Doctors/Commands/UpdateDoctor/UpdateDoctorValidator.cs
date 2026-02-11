using FluentValidation;

namespace CMS.Application.Doctors.Commands.UpdateDoctor;

public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorCommand>
{
    public UpdateDoctorValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Doctor ID is required.");
        
        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License Number is required.")
            .MaximumLength(50).WithMessage("License Number cannot exceed 50 characters.");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required.")
            .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters.");
    }
}
