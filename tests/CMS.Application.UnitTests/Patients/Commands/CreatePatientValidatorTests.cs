using CMS.Application.Patients.Commands.CreatePatient;
using CMS.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Patients.Commands;

public class CreatePatientValidatorTests
{
    private readonly CreatePatientValidator _validator;

    public CreatePatientValidatorTests()
    {
        _validator = new CreatePatientValidator();
    }

    [Fact]
    public void Should_HaveError_When_UserIdIsEmpty()
    {
        var command = new CreatePatientCommand(
            UserId: Guid.Empty,
            NationalId: "123",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_HaveError_When_NationalIdIsEmpty()
    {
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NationalId);
    }

    [Fact]
    public void Should_HaveError_When_NationalIdExceedsMaxLength()
    {
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: new string('1', 21),
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NationalId);
    }

    [Fact]
    public void Should_HaveError_When_DateOfBirthIsInFuture()
    {
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "123",
            DateOfBirth: DateTime.UtcNow.AddDays(1),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
    }

    [Fact]
    public void Should_HaveError_When_PhoneIsEmpty()
    {
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "123",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "",
            EmergencyContact: "Parent"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Phone);
    }

    [Fact]
    public void Should_HaveError_When_GenderIsInvalid()
    {
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "123",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: (Gender)999,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Gender);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "123",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
