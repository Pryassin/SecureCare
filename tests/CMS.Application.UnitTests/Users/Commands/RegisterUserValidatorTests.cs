using CMS.Application.Users.Commands.RegisterUser;
using CMS.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Users.Commands;

public class RegisterUserValidatorTests
{
    private readonly RegisterUserValidator _validator;

    public RegisterUserValidatorTests()
    {
        _validator = new RegisterUserValidator();
    }

    [Fact]
    public void Should_HaveError_When_EmailIsEmpty()
    {
        var command = new RegisterUserCommand("", "Password123!", UserRole.Patient);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_HaveError_When_EmailIsInvalid()
    {
        var command = new RegisterUserCommand("notanemail", "Password123!", UserRole.Patient);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_HaveError_When_PasswordIsEmpty()
    {
        var command = new RegisterUserCommand("test@test.com", "", UserRole.Patient);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_HaveError_When_PasswordIsTooShort()
    {
        var command = new RegisterUserCommand("test@test.com", "12345", UserRole.Patient);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_HaveError_When_RoleIsInvalid()
    {
        var command = new RegisterUserCommand("test@test.com", "Password123!", (UserRole)999);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Role);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new RegisterUserCommand("test@test.com", "Password123!", UserRole.Patient);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
