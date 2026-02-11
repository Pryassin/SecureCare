using FluentValidation;

namespace CMS.Application.Doctors.Commands.CreateDoctor;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        
        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("License Number is required.")
            .MaximumLength(50).WithMessage("License Number cannot exceed 50 characters.");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required.")
            .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters.");
    }
}
