using CMS.Application.Doctors.Commands.CreateDoctor;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Doctors.Commands;

public class CreateDoctorValidatorTests
{
    private readonly CreateDoctorValidator _validator;

    public CreateDoctorValidatorTests()
    {
        _validator = new CreateDoctorValidator();
    }

    [Fact]
    public void Should_HaveError_When_UserIdIsEmpty()
    {
        var command = new CreateDoctorCommand(Guid.Empty, "License123", "Cardiology");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_HaveError_When_LicenseNumberIsEmpty()
    {
        var command = new CreateDoctorCommand(Guid.NewGuid(), "", "Cardiology");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LicenseNumber);
    }

    [Fact]
    public void Should_HaveError_When_LicenseNumberExceedsMaxLength()
    {
        var command = new CreateDoctorCommand(Guid.NewGuid(), new string('A', 51), "Cardiology");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LicenseNumber);
    }

    [Fact]
    public void Should_HaveError_When_SpecializationIsEmpty()
    {
        var command = new CreateDoctorCommand(Guid.NewGuid(), "License123", "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Specialization);
    }

    [Fact]
    public void Should_HaveError_When_SpecializationExceedsMaxLength()
    {
        var command = new CreateDoctorCommand(Guid.NewGuid(), "License123", new string('A', 101));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Specialization);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new CreateDoctorCommand(Guid.NewGuid(), "License123", "Cardiology");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
