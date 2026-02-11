using CMS.Application.Users.Commands.Login;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace CMS.Application.UnitTests.Users.Commands;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator;

    public LoginValidatorTests()
    {
        _validator = new LoginValidator();
    }

    [Fact]
    public void Should_HaveError_When_EmailIsEmpty()
    {
        var command = new LoginCommand("", "password");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_HaveError_When_EmailIsInvalid()
    {
        var command = new LoginCommand("notanemail", "password");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_HaveError_When_PasswordIsEmpty()
    {
        var command = new LoginCommand("test@test.com", "");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_NotHaveError_When_CommandIsValid()
    {
        var command = new LoginCommand("test@test.com", "password");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
