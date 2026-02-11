using CMS.Domain.Entities;
using CMS.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Entities;

public class AppointmentTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var dateTime = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var result = Appointment.Create(patientId, doctorId, dateTime);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.PatientId.Should().Be(patientId);
        result.Value.DoctorId.Should().Be(doctorId);
        result.Value.DateTime.Should().Be(dateTime);
        result.Value.Status.Should().Be(AppointmentStatus.Confirmed);
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenDateTimeIsInPast()
    {
        // Act
        var result = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(-1));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Appointment.PastDate");
    }

    [Fact]
    public void UpdateStatus_Should_ReturnSuccess_WhenStatusIsValid()
    {
        // Arrange
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1)).Value;

        // Act
        var result = appointment.UpdateStatus(AppointmentStatus.Completed);

        // Assert
        result.IsSuccess.Should().BeTrue();
        appointment.Status.Should().Be(AppointmentStatus.Completed);
    }

    [Fact]
    public void UpdateStatus_Should_AllowCancellation()
    {
        // Arrange
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1)).Value;

        // Act
        var result = appointment.UpdateStatus(AppointmentStatus.Cancelled);

        // Assert
        result.IsSuccess.Should().BeTrue();
        appointment.Status.Should().Be(AppointmentStatus.Cancelled);
    }

    [Theory]
    [InlineData(AppointmentStatus.Confirmed)]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Cancelled)]
    public void UpdateStatus_Should_AcceptAllValidStatuses(AppointmentStatus status)
    {
        // Arrange
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1)).Value;

        // Act
        var result = appointment.UpdateStatus(status);

        // Assert
        result.IsSuccess.Should().BeTrue();
        appointment.Status.Should().Be(status);
    }

    [Fact]
    public void Reschedule_Should_ReturnSuccess_WhenDateTimeIsValid()
    {
        // Arrange
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1)).Value;
        var newDateTime = DateTimeOffset.UtcNow.AddDays(2);

        // Act
        var result = appointment.Reschedule(newDateTime);

        // Assert
        result.IsSuccess.Should().BeTrue();
        appointment.DateTime.Should().Be(newDateTime);
    }

    [Fact]
    public void Reschedule_Should_ReturnFailure_WhenDateTimeIsInPast()
    {
        // Arrange
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(1)).Value;

        // Act
        var result = appointment.Reschedule(DateTimeOffset.UtcNow.AddDays(-1));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Appointment.PastDate");
    }
}
