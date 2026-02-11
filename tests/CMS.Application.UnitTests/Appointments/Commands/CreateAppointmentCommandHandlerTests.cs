using CMS.Application.Appointments.Commands.CreateAppointment;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Appointments.Commands;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateAppointmentCommandHandler _handler;

    public CreateAppointmentCommandHandlerTests()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateAppointmentCommandHandler(_appointmentRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenDoctorIsNotAvailable()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PatientId: Guid.NewGuid(),
            DoctorId: Guid.NewGuid(),
            DateTime: DateTime.UtcNow.AddDays(1)
        );

        // Mock repository to return FALSE for IsDoctorAvailableAsync
        _appointmentRepositoryMock.Setup(repo => 
                repo.IsDoctorAvailableAsync(command.DoctorId, command.DateTime, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // Doctor is busy!

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Appointment.Conflict");
        _appointmentRepositoryMock.Verify(x => x.Add(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDoctorIsAvailable()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            PatientId: Guid.NewGuid(),
            DoctorId: Guid.NewGuid(),
            DateTime: DateTime.UtcNow.AddDays(1)
        );

        // Mock repository to return TRUE (Available)
        _appointmentRepositoryMock.Setup(repo => 
                repo.IsDoctorAvailableAsync(command.DoctorId, command.DateTime, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        _appointmentRepositoryMock.Verify(x => x.Add(It.Is<Appointment>(a => 
            a.PatientId == command.PatientId &&
            a.DoctorId == command.DoctorId
        )), Times.Once);

        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
