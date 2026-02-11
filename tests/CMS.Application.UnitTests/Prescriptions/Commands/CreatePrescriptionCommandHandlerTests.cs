using CMS.Application.Prescriptions.Commands.CreatePrescription;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Enums;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Prescriptions.Commands;

public class CreatePrescriptionCommandHandlerTests
{
    private readonly Mock<IPrescriptionRepository> _prescriptionRepositoryMock;
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreatePrescriptionCommandHandler _handler;

    public CreatePrescriptionCommandHandlerTests()
    {
        _prescriptionRepositoryMock = new Mock<IPrescriptionRepository>();
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreatePrescriptionCommandHandler(
            _prescriptionRepositoryMock.Object, 
            _appointmentRepositoryMock.Object, 
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenAppointmentIsNotFound()
    {
        // Arrange
        var command = new CreatePrescriptionCommand(
            AppointmentId: Guid.NewGuid(),
            MedicationDetails: "Med 1",
            DoctorNotes: "Notes"
        );

        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(command.AppointmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Appointment.NotFound");
    }

    // Since Appointment Entity is quite protected (private setters),
    // creating a "Completed" appointment via factory involves ensuring validation passes.
    // However, the Appointment factory enforces strict rules.
    // It's tricky to mock the Appointment's 'Status' property unless we use reflection 
    // or if the repository mock can return an object where valid business rules apply.
    // 
    // In Unit Tests with EF Core entities, creating entities in specific states can be hard 
    // if the logic is encapsulated. 
    // 
    // Here, I will assume Appointment.Create sets status to "Confirmed".
    // I need to confirm if I can change status to "Completed" easily in tests without full workflow.
    // Appointment.UpdateStatus() method exists!

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenAppointmentIsNotCompleted()
    {
        // Arrange
        var command = new CreatePrescriptionCommand(
            AppointmentId: Guid.NewGuid(),
            MedicationDetails: "Med 1",
            DoctorNotes: "Notes"
        );

        // Create a confirmed appointment (Status = Confirmed by default)
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1)).Value;
        
        // Ensure it is NOT completed (it is Confirmed)
        appointment.Status.Should().Be(AppointmentStatus.Confirmed);

        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(command.AppointmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Prescription.InvalidStatus");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenAppointmentIsCompleted()
    {
        // Arrange
        var command = new CreatePrescriptionCommand(
            AppointmentId: Guid.NewGuid(),
            MedicationDetails: "Med 1",
            DoctorNotes: "Notes"
        );

        // Create and complete appointment
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1)).Value;
        appointment.UpdateStatus(AppointmentStatus.Completed); 
        
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(command.AppointmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        _prescriptionRepositoryMock.Verify(x => x.Add(It.Is<Prescription>(p => 
            p.AppointmentId == command.AppointmentId &&
            p.MedicationDetails == command.MedicationDetails
        )), Times.Once);
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
