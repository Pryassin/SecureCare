using CMS.Application.Patients.Commands.CreatePatient;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Enums;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Patients.Commands;

public class CreatePatientCommandHandlerTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreatePatientCommandHandler _handler;

    public CreatePatientCommandHandlerTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreatePatientCommandHandler(_patientRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUserAlreadyHasPatientProfile()
    {
        // Arrange
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "1234567890",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        // Simulate existing patient for this User ID
        // Note: Since Patient constructor is private, we can't easily create a dummy one here without factory or reflection.
        // However, the repository returns a Patient.
        // We can use the static Create method to generate a dummy patient for the mock return.
        var dummyPatient = Patient.Create(
            command.UserId, 
            "ExistingNatID", 
            DateTime.UtcNow, 
            Gender.Female, 
            "ExistingPhone", 
            null
        ).Value;

        _patientRepositoryMock.Setup(repo => repo.GetByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dummyPatient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Patient.AlreadyExists");
        _patientRepositoryMock.Verify(x => x.Add(It.IsAny<Patient>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenNationalIdIsDuplicate()
    {
        // Arrange
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "DuplicateNatID",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        _patientRepositoryMock.Setup(repo => repo.GetByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null); // No existing patient for this user

        var dummyConflictPatient = Patient.Create(
            Guid.NewGuid(), 
            command.NationalId, 
            DateTime.UtcNow, 
            Gender.Female, 
            "OtherPhone", 
            null
        ).Value;

        _patientRepositoryMock.Setup(repo => repo.GetByNationalIdAsync(command.NationalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dummyConflictPatient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Patient.DuplicateNationalId");
        _patientRepositoryMock.Verify(x => x.Add(It.IsAny<Patient>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var command = new CreatePatientCommand(
            UserId: Guid.NewGuid(),
            NationalId: "UniqueNatID",
            DateOfBirth: DateTime.UtcNow.AddYears(-30),
            Gender: Gender.Male,
            Phone: "555-0100",
            EmergencyContact: "Parent"
        );

        _patientRepositoryMock.Setup(repo => repo.GetByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        _patientRepositoryMock.Setup(repo => repo.GetByNationalIdAsync(command.NationalId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        _patientRepositoryMock.Verify(x => x.Add(It.Is<Patient>(p => 
            p.UserId == command.UserId &&
            p.NationalId == command.NationalId
        )), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
