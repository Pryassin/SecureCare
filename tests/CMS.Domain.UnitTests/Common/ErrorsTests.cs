using CMS.Domain.Common;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Common;

public class ErrorsTests
{
    [Fact]
    public void User_NotFound_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.User.NotFound;

        // Assert
        error.Code.Should().Be("User.NotFound");
        error.Description.Should().Be("User not found");
    }

    [Fact]
    public void User_AlreadyExists_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.User.AlreadyExists;

        // Assert
        error.Code.Should().Be("User.AlreadyExists");
        error.Description.Should().Be("User already exists");
    }

    [Fact]
    public void User_EmailRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.User.EmailRequired;

        // Assert
        error.Code.Should().Be("User.EmailRequired");
        error.Description.Should().Be("Email is required");
    }

    [Fact]
    public void User_PasswordRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.User.PasswordRequired;

        // Assert
        error.Code.Should().Be("User.PasswordRequired");
        error.Description.Should().Be("Password is Required");
    }

    [Fact]
    public void Doctor_NotFound_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Doctor.NotFound;

        // Assert
        error.Code.Should().Be("Doctor.NotFound");
        error.Description.Should().Be("Doctor not found");
    }

    [Fact]
    public void Doctor_AlreadyExists_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Doctor.AlreadyExists;

        // Assert
        error.Code.Should().Be("Doctor.AlreadyExists");
        error.Description.Should().Be("Doctor profile already exists for this user");
    }

    [Fact]
    public void Doctor_UserIdRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Doctor.UserIdRequired;

        // Assert
        error.Code.Should().Be("Doctor.UserIdRequired");
        error.Description.Should().Contain("User ID");
    }

    [Fact]
    public void Doctor_LicenseRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Doctor.LicenseRequired;

        // Assert
        error.Code.Should().Be("Doctor.LicenseRequired");
        error.Description.Should().Contain("license");
    }

    [Fact]
    public void Doctor_SpecializationRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Doctor.SpecializationRequired;

        // Assert
        error.Code.Should().Be("Doctor.SpecializationRequired");
        error.Description.Should().Contain("specialization");
    }

    [Fact]
    public void Patient_NotFound_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Patient.NotFound;

        // Assert
        error.Code.Should().Be("Patient.NotFound");
        error.Description.Should().Be("Patient not found");
    }

    [Fact]
    public void Patient_AlreadyExists_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Patient.AlreadyExists;

        // Assert
        error.Code.Should().Be("Patient.AlreadyExists");
        error.Description.Should().Be("Patient profile already exists for this user");
    }

    [Fact]
    public void Patient_DuplicateNationalId_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Patient.DuplicateNationalId;

        // Assert
        error.Code.Should().Be("Patient.DuplicateNationalId");
        error.Description.Should().Be("A patient with this national ID already exists");
    }

    [Fact]
    public void Patient_UserIdRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Patient.UserIdRequired;

        // Assert
        error.Code.Should().Be("Patient.UserIdRequired");
        error.Description.Should().Be("User ID is required.");
    }

    [Fact]
    public void Patient_NationalIdRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Patient.NationalIdRequired;

        // Assert
        error.Code.Should().Be("Patient.NationalIdRequired");
        error.Description.Should().Be("National ID is required.");
    }

    [Fact]
    public void Patient_PhoneRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Patient.PhoneRequired;

        // Assert
        error.Code.Should().Be("Patient.PhoneRequired");
        error.Description.Should().Be("Phone number is required.");
    }

    [Fact]
    public void Appointment_PatientIdRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Appointment.PatientIdRequired;

        // Assert
        error.Code.Should().Be("Appointment.PatientIdRequired");
        error.Description.Should().Contain("Patient ID");
    }

    [Fact]
    public void Appointment_DoctorIdRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Appointment.DoctorIdRequired;

        // Assert
        error.Code.Should().Be("Appointment.DoctorIdRequired");
        error.Description.Should().Contain("Doctor ID");
    }

    [Fact]
    public void Appointment_PastDate_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Appointment.PastDate;

        // Assert
        error.Code.Should().Be("Appointment.PastDate");
        error.Description.Should().Contain("past");
    }

    [Fact]
    public void Appointment_Conflict_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Appointment.Conflict;

        // Assert
        error.Code.Should().Be("Appointment.Conflict");
        error.Description.Should().Contain("booked");
    }

    [Fact]
    public void Appointment_NotFound_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Appointment.NotFound;

        // Assert
        error.Code.Should().Be("Appointment.NotFound");
        error.Description.Should().Be("Appointment not found.");
    }

    [Fact]
    public void Prescription_NotFound_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Prescription.NotFound;

        // Assert
        error.Code.Should().Be("Prescription.NotFound");
        error.Description.Should().Be("Prescription not found");
    }

    [Fact]
    public void Prescription_InvalidStatus_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Prescription.InvalidStatus;

        // Assert
        error.Code.Should().Be("Prescription.InvalidStatus");
        error.Description.Should().Be("Prescription can only be created for completed appointments");
    }

    [Fact]
    public void Prescription_AppointmentIdRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Prescription.AppointmentIdRequired;

        // Assert
        error.Code.Should().Be("Prescription.AppointmentIdRequired");
        error.Description.Should().Be("Appointment ID is required.");
    }

    [Fact]
    public void Prescription_MedicationDetailsRequired_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Prescription.MedicationDetailsRequired;

        // Assert
        error.Code.Should().Be("Prescription.MedicationDetailsRequired");
        error.Description.Should().Be("Medication details are required.");
    }

    [Fact]
    public void Auth_InvalidCredentials_Should_HaveCorrectCodeAndDescription()
    {
        // Act
        var error = Errors.Auth.InvalidCredentials;

        // Assert
        error.Code.Should().Be("Auth.InvalidCredentials");
        error.Description.Should().Be("Invalid email or password");
    }
}
