using CMS.Application.Prescriptions.Commands.CreatePrescription;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Prescriptions.Commands;

public class CreatePrescriptionValidatorTests
{
    private readonly CreatePrescriptionValidator _validator;

    public CreatePrescriptionValidatorTests()
    {
        _validator = new CreatePrescriptionValidator();
    }

    [Fact]
    public void Should_HaveError_When_AppointmentIdIsEmpty()
    {
        var command = new CreatePrescriptionCommand(Guid.Empty, "Meds", "Notes");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.AppointmentId);
    }

    [Fact]
    public void Should_HaveError_When_MedicationDetailsIsEmpty()
    {
        var command = new CreatePrescriptionCommand(Guid.NewGuid(), "", "Notes");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.MedicationDetails);
    }

    [Fact]
    public void Should_HaveError_When_MedicationDetailsExceedsMaxLength()
    {
        var command = new CreatePrescriptionCommand(Guid.NewGuid(), new string('A', 1001), "Notes");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.MedicationDetails);
    }

    [Fact]
    public void Should_HaveError_When_DoctorNotesExceedsMaxLength()
    {
        var command = new CreatePrescriptionCommand(Guid.NewGuid(), "Meds", new string('A', 1001));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.DoctorNotes);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new CreatePrescriptionCommand(Guid.NewGuid(), "Meds", "Notes");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_NotHaveError_When_DoctorNotesIsNull()
    {
        var command = new CreatePrescriptionCommand(Guid.NewGuid(), "Meds", null);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
