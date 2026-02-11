using CMS.Application.Doctors.Commands.UpdateDoctor;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Doctors.Commands;

public class UpdateDoctorValidatorTests
{
    private readonly UpdateDoctorValidator _validator;

    public UpdateDoctorValidatorTests()
    {
        _validator = new UpdateDoctorValidator();
    }

    [Fact]
    public void Should_HaveError_When_IdIsEmpty()
    {
        var command = new UpdateDoctorCommand(Guid.Empty, "License", "Spec");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_HaveError_When_LicenseNumberIsEmpty()
    {
        var command = new UpdateDoctorCommand(Guid.NewGuid(), "", "Spec");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LicenseNumber);
    }

    [Fact]
    public void Should_HaveError_When_SpecializationIsEmpty()
    {
        var command = new UpdateDoctorCommand(Guid.NewGuid(), "License", "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Specialization);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new UpdateDoctorCommand(Guid.NewGuid(), "License", "Spec");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
