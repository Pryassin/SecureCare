using CMS.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Entities;

public class PrescriptionTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var medicationDetails = "Medication ABC";
        var doctorNotes = "Take twice daily";

        // Act
        var result = Prescription.Create(appointmentId, medicationDetails, doctorNotes);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.AppointmentId.Should().Be(appointmentId);
        result.Value.MedicationDetails.Should().Be(medicationDetails);
        result.Value.DoctorNotes.Should().Be(doctorNotes);
        result.Value.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenMedicationDetailsIsEmpty()
    {
        // Act
        var result = Prescription.Create(Guid.NewGuid(), "", "Notes");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Prescription.MedicationDetailsRequired");
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenMedicationDetailsIsWhitespace()
    {
        // Act
        var result = Prescription.Create(Guid.NewGuid(), "   ", "Notes");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Prescription.MedicationDetailsRequired");
    }

    [Fact]
    public void Create_Should_AllowNullDoctorNotes()
    {
        // Act
        var result = Prescription.Create(Guid.NewGuid(), "Medication", null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DoctorNotes.Should().BeNull();
    }

    [Fact]
    public void Create_Should_AllowEmptyDoctorNotes()
    {
        // Act
        var result = Prescription.Create(Guid.NewGuid(), "Medication", "");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DoctorNotes.Should().BeEmpty();
    }

    [Fact]
    public void Create_Should_SetCreatedAtToCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var result = Prescription.Create(Guid.NewGuid(), "Medication", "Notes");

        // Assert
        var afterCreation = DateTimeOffset.UtcNow;
        result.Value.CreatedAt.Should().BeOnOrAfter(beforeCreation);
        result.Value.CreatedAt.Should().BeOnOrBefore(afterCreation);
    }
}
