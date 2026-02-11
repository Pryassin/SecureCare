using CMS.Domain.Common;
using FluentAssertions;
using Xunit;

namespace CMS.Domain.UnitTests.Common;

public class ResultTests
{
    [Fact]
    public void Success_Should_CreateSuccessResult()
    {
        // Act
        var result = Result.Success;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Failure_Should_CreateFailureResult()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error message");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void GenericSuccess_Should_CreateSuccessResultWithValue()
    {
        // Arrange
        var value = "test value";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(value);
    }

    [Fact]
    public void GenericFailure_Should_CreateFailureResultWithError()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error message");

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_Should_CreateSuccessResult()
    {
        // Act
        Result<int> result = 42;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void ImplicitConversion_FromError_Should_CreateFailureResult()
    {
        // Arrange
        var error = new Error("Test.Error", "Test error message");

        // Act
        Result<int> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Error_Should_HaveCodeAndDescription()
    {
        // Arrange
        var code = "Test.Error";
        var description = "This is a test error";

        // Act
        var error = new Error(code, description);

        // Assert
        error.Code.Should().Be(code);
        error.Description.Should().Be(description);
    }

    [Fact]
    public void Error_Equality_Should_CompareByCodeAndDescription()
    {
        // Arrange
        var error1 = new Error("Test.Error", "Description");
        var error2 = new Error("Test.Error", "Description");
        var error3 = new Error("Different.Error", "Description");

        // Assert
        error1.Should().Be(error2);
        error1.Should().NotBe(error3);
    }
}
