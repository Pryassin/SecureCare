using CMS.Application.Patients.Commands.UpdatePatient;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Patients.Commands;

public class UpdatePatientCommandHandlerTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdatePatientCommandHandler _handler;

    public UpdatePatientCommandHandlerTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdatePatientCommandHandler(_patientRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPatientNotFound()
    {
        // Arrange
        var command = new UpdatePatientCommand(Guid.NewGuid(), "NewNatID", "NewPhone", "NewContact");
        
        _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        // Assuming the error code used is User.NotFound based on current implementation
        result.Error.Code.Should().Be("User.NotFound"); 
        
        _patientRepositoryMock.Verify(x => x.Update(It.IsAny<Patient>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenUpdateDetailsAreValid()
    {
        // Arrange
        var command = new UpdatePatientCommand(Guid.NewGuid(), "NewNatID", "NewPhone", "NewContact");
        
        // Mock existing patient
        var patient = Patient.Create(Guid.NewGuid(), "OldNatID", DateTime.UtcNow, CMS.Domain.Enums.Gender.Male, "OldPhone", "OldContact").Value;
        
        // Since Patient.Create generates a NEW ID, we might need to set the command ID to match, 
        // OR rely on Repo mocking to return this patient regardless of ID passed.
        // But the command expects a specific ID.
        // Ideally, we can't 'set' the ID on the entity easily if protected. 
        // So we just mock the repo to return THIS patient when asked for command.Id.
        
        _patientRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        patient.NationalId.Should().Be(command.NationalId);
        patient.Phone.Should().Be(command.Phone);
        patient.EmergencyContact.Should().Be(command.EmergencyContact);

        _patientRepositoryMock.Verify(x => x.Update(patient), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
