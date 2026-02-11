using CMS.Application.Common.Interfaces;
using CMS.Application.Users.Commands.Login;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Enums;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Users.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IJwtTokenGenerator> _jwtMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _jwtMock = new Mock<IJwtTokenGenerator>();
        _handler = new LoginCommandHandler(_userRepositoryMock.Object, _jwtMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUserNotFound()
    {
        var command = new LoginCommand("unknown@test.com", "pass");

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.InvalidCredentials");
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPasswordIsIncorrect()
    {
        var correctPass = "SecurePass1!";
        var wrongPass = "WrongPass1!";
        
        // Setup User with specific hashed password
        string validHash = BCrypt.Net.BCrypt.HashPassword(correctPass);
        var user = User.Create("user@test.com", validHash, UserRole.Patient).Value;

        var command = new LoginCommand("user@test.com", wrongPass);

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.InvalidCredentials");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessToken_WhenCredentialsAreValid()
    {
        var password = "SecurePass1!";
        string validHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = User.Create("user@test.com", validHash, UserRole.Doctor).Value;

        var command = new LoginCommand("user@test.com", password);

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _jwtMock.Setup(x => x.GenerateToken(user)).Returns("valid.jwt.token");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("valid.jwt.token");
    }
}
