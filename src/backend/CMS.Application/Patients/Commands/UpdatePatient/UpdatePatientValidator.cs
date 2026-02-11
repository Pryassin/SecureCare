using FluentValidation;

namespace CMS.Application.Patients.Commands.UpdatePatient;

public class UpdatePatientValidator : AbstractValidator<UpdatePatientCommand>
{
    public UpdatePatientValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Patient ID is required.");
        
        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required.")
            .MaximumLength(20).WithMessage("National ID cannot exceed 20 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");
    }
}
