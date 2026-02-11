using CMS.Application.Patients.Commands.UpdatePatient;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Patients.Commands;

public class UpdatePatientValidatorTests
{
    private readonly UpdatePatientValidator _validator;

    public UpdatePatientValidatorTests()
    {
        _validator = new UpdatePatientValidator();
    }

    [Fact]
    public void Should_HaveError_When_IdIsEmpty()
    {
        var command = new UpdatePatientCommand(Guid.Empty, "123", "555", "Contact");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_HaveError_When_NationalIdIsEmpty()
    {
        var command = new UpdatePatientCommand(Guid.NewGuid(), "", "555", "Contact");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NationalId);
    }

    [Fact]
    public void Should_HaveError_When_PhoneIsEmpty()
    {
        var command = new UpdatePatientCommand(Guid.NewGuid(), "123", "", "Contact");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new UpdatePatientCommand(Guid.NewGuid(), "123", "555", "Contact");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
