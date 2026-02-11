using CMS.Domain.Entities;
using CMS.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Entities;

public class PatientTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var nationalId = "123456";
        var dob = DateTime.UtcNow.AddYears(-30);
        var gender = Gender.Male;
        var phone = "555-0100";
        var emergencyContact = "Parent";

        // Act
        var result = Patient.Create(userId, nationalId, dob, gender, phone, emergencyContact);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.UserId.Should().Be(userId);
        result.Value.NationalId.Should().Be(nationalId);
        result.Value.DateOfBirth.Should().Be(dob);
        result.Value.Gender.Should().Be(gender);
        result.Value.Phone.Should().Be(phone);
        result.Value.EmergencyContact.Should().Be(emergencyContact);
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenNationalIdIsEmpty()
    {
        // Act
        var result = Patient.Create(Guid.NewGuid(), "", DateTime.UtcNow, Gender.Male, "555", null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Patient.NationalIdRequired");
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenPhoneIsEmpty()
    {
        // Act
        var result = Patient.Create(Guid.NewGuid(), "123", DateTime.UtcNow, Gender.Male, "", null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Patient.PhoneRequired");
    }

    [Fact]
    public void Create_Should_AllowNullEmergencyContact()
    {
        // Act
        var result = Patient.Create(Guid.NewGuid(), "123", DateTime.UtcNow.AddYears(-20), Gender.Female, "555", null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.EmergencyContact.Should().BeNull();
    }

    [Fact]
    public void Update_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var patient = Patient.Create(Guid.NewGuid(), "123", DateTime.UtcNow.AddYears(-20), Gender.Male, "555", "Old").Value;
        var newNationalId = "456";
        var newPhone = "777";
        var newContact = "New";

        // Act
        var result = patient.Update(newNationalId, newPhone, newContact);

        // Assert
        result.IsSuccess.Should().BeTrue();
        patient.NationalId.Should().Be(newNationalId);
        patient.Phone.Should().Be(newPhone);
        patient.EmergencyContact.Should().Be(newContact);
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenNationalIdIsEmpty()
    {
        // Arrange
        var patient = Patient.Create(Guid.NewGuid(), "123", DateTime.UtcNow.AddYears(-20), Gender.Male, "555", null).Value;

        // Act
        var result = patient.Update("", "777", null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Patient.NationalIdRequired");
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenPhoneIsEmpty()
    {
        // Arrange
        var patient = Patient.Create(Guid.NewGuid(), "123", DateTime.UtcNow.AddYears(-20), Gender.Male, "555", null).Value;

        // Act
        var result = patient.Update("456", "", null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Patient.PhoneRequired");
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    public void Create_Should_AcceptAllValidGenders(Gender gender)
    {
        // Act
        var result = Patient.Create(Guid.NewGuid(), "123", DateTime.UtcNow.AddYears(-20), gender, "555", null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Gender.Should().Be(gender);
    }
}
