using FluentValidation;

namespace CMS.Application.Patients.Commands.CreatePatient;

public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        
        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required.")
            .MaximumLength(20).WithMessage("National ID cannot exceed 20 characters.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

        RuleFor(x => x.Gender).IsInEnum().WithMessage("Invalid gender.");
    }
}
