using CMS.Application.Appointments.Commands.CreateAppointment;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Appointments.Commands;

public class CreateAppointmentValidatorTests
{
    private readonly CreateAppointmentValidator _validator;

    public CreateAppointmentValidatorTests()
    {
        _validator = new CreateAppointmentValidator();
    }

    [Fact]
    public void Should_HaveError_When_PatientIdIsEmpty()
    {
        var command = new CreateAppointmentCommand(
            PatientId: Guid.Empty,
            DoctorId: Guid.NewGuid(),
            DateTime: DateTime.UtcNow.AddDays(1)
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PatientId);
    }

    [Fact]
    public void Should_HaveError_When_DoctorIdIsEmpty()
    {
        var command = new CreateAppointmentCommand(
            PatientId: Guid.NewGuid(),
            DoctorId: Guid.Empty,
            DateTime: DateTime.UtcNow.AddDays(1)
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DoctorId);
    }

    [Fact]
    public void Should_HaveError_When_DateTimeIsInPast()
    {
        var command = new CreateAppointmentCommand(
            PatientId: Guid.NewGuid(),
            DoctorId: Guid.NewGuid(),
            DateTime: DateTime.UtcNow.AddDays(-1)
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DateTime);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new CreateAppointmentCommand(
            PatientId: Guid.NewGuid(),
            DoctorId: Guid.NewGuid(),
            DateTime: DateTime.UtcNow.AddDays(1)
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
