using CMS.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Entities;

public class DoctorTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var license = "LIC123";
        var specialization = "Cardiology";

        // Act
        var result = Doctor.Create(userId, license, specialization);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be(userId);
        result.Value.LicenseNumber.Should().Be(license);
        result.Value.Specialization.Should().Be(specialization);
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenLicenseNumberIsEmpty()
    {
        // Act
        var result = Doctor.Create(Guid.NewGuid(), "", "Cardiology");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Doctor.LicenseRequired");
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenSpecializationIsEmpty()
    {
        // Act
        var result = Doctor.Create(Guid.NewGuid(), "LIC123", "");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Doctor.SpecializationRequired");
    }

    [Fact]
    public void Update_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var doctor = Doctor.Create(Guid.NewGuid(), "LIC123", "Cardiology").Value;
        var newLicense = "LIC456";
        var newSpec = "Neurology";

        // Act
        var result = doctor.Update(newLicense, newSpec);

        // Assert
        result.IsSuccess.Should().BeTrue();
        doctor.LicenseNumber.Should().Be(newLicense);
        doctor.Specialization.Should().Be(newSpec);
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenLicenseNumberIsEmpty()
    {
        // Arrange
        var doctor = Doctor.Create(Guid.NewGuid(), "LIC123", "Cardiology").Value;

        // Act
        var result = doctor.Update("", "Neurology");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Doctor.LicenseRequired");
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenSpecializationIsEmpty()
    {
        // Arrange
        var doctor = Doctor.Create(Guid.NewGuid(), "LIC123", "Cardiology").Value;

        // Act
        var result = doctor.Update("LIC456", "");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Doctor.SpecializationRequired");
    }
}
