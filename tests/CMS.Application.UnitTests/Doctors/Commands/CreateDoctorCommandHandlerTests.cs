using CMS.Application.Doctors.Commands.CreateDoctor;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Doctors.Commands;

public class CreateDoctorCommandHandlerTests
{
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateDoctorCommandHandler _handler;

    public CreateDoctorCommandHandlerTests()
    {
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateDoctorCommandHandler(_doctorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenDoctorProfileAlreadyExistsForUser()
    {
        // Arrange
        var command = new CreateDoctorCommand(Guid.NewGuid(), "License123", "Cardiology");

        // Mock existing doctor
        var existingDoctor = Doctor.Create(command.UserId, "OldLicense", "OldSpec").Value;
        
        _doctorRepositoryMock.Setup(repo => repo.GetByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDoctor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Doctor.AlreadyExists");
        _doctorRepositoryMock.Verify(x => x.Add(It.IsAny<Doctor>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        var command = new CreateDoctorCommand(Guid.NewGuid(), "License123", "Cardiology");

        _doctorRepositoryMock.Setup(repo => repo.GetByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        _doctorRepositoryMock.Verify(x => x.Add(It.Is<Doctor>(d => 
            d.UserId == command.UserId &&
            d.LicenseNumber == command.LicenseNumber
        )), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
