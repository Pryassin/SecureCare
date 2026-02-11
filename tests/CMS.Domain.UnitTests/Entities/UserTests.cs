using CMS.Domain.Entities;
using CMS.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Entities;

public class UserTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var email = "test@test.com";
        var passwordHash = "hashedpassword";
        var role = UserRole.Patient;

        // Act
        var result = User.Create(email, passwordHash, role);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be(email);
        result.Value.PasswordHash.Should().Be(passwordHash);
        result.Value.Role.Should().Be(role);
        result.Value.IsActive.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenEmailIsEmpty()
    {
        // Act
        var result = User.Create("", "hash", UserRole.Patient);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.EmailRequired");
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenPasswordHashIsEmpty()
    {
        // Act
        var result = User.Create("test@test.com", "", UserRole.Patient);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.PasswordRequired");
    }

    [Fact]
    public void Create_Should_ReturnFailure_WhenRoleIsInvalid()
    {
        // Act
        var result = User.Create("test@test.com", "hash", (UserRole)999);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.InvalidRole");
    }

    [Fact]
    public void Update_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var user = User.Create("test@test.com", "hash", UserRole.Patient).Value;
        var newEmail = "newemail@test.com";
        var newRole = UserRole.Doctor;

        // Act
        var result = user.Update(newEmail, newRole, true);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Email.Should().Be(newEmail);
        user.Role.Should().Be(newRole);
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenEmailIsEmpty()
    {
        // Arrange
        var user = User.Create("test@test.com", "hash", UserRole.Patient).Value;

        // Act
        var result = user.Update("", UserRole.Patient, true);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.EmailRequired");
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenEmailIsWhitespace()
    {
        // Arrange
        var user = User.Create("test@test.com", "hash", UserRole.Patient).Value;

        // Act
        var result = user.Update("   ", UserRole.Patient, true);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.EmailRequired");
    }

    [Fact]
    public void Update_Should_ReturnFailure_WhenRoleIsInvalid()
    {
        // Arrange
        var user = User.Create("test@test.com", "hash", UserRole.Patient).Value;

        // Act
        var result = user.Update("test@test.com", (UserRole)999, true);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.InvalidRole");
    }

    [Fact]
    public void Update_Should_AllowDeactivatingUser()
    {
        // Arrange
        var user = User.Create("test@test.com", "hash", UserRole.Patient).Value;

        // Act
        var result = user.Update("test@test.com", UserRole.Patient, false);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Update_Should_AllowChangingRole()
    {
        // Arrange
        var user = User.Create("test@test.com", "hash", UserRole.Patient).Value;

        // Act
        var result = user.Update("test@test.com", UserRole.Admin, true);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Role.Should().Be(UserRole.Admin);
    }

    [Theory]
    [InlineData(UserRole.Patient)]
    [InlineData(UserRole.Doctor)]
    [InlineData(UserRole.Admin)]
    public void Create_Should_AcceptAllValidRoles(UserRole role)
    {
        // Act
        var result = User.Create("test@test.com", "hash", role);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Role.Should().Be(role);
    }
}
