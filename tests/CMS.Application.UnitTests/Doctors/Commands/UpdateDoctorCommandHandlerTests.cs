using CMS.Application.Doctors.Commands.UpdateDoctor;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Doctors.Commands;

public class UpdateDoctorCommandHandlerTests
{
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateDoctorCommandHandler _handler;

    public UpdateDoctorCommandHandlerTests()
    {
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateDoctorCommandHandler(_doctorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenDoctorNotFound()
    {
        var command = new UpdateDoctorCommand(Guid.NewGuid(), "NewLicense", "NewSpec");
        
        _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenUpdateDetailsAreValid()
    {
        var command = new UpdateDoctorCommand(Guid.NewGuid(), "NewLicense", "NewSpec");
        
        var doctor = Doctor.Create(Guid.NewGuid(), "OldLicense", "OldSpec").Value;
        
        _doctorRepositoryMock.Setup(repo => repo.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        doctor.LicenseNumber.Should().Be(command.LicenseNumber);
        doctor.Specialization.Should().Be(command.Specialization);

        _doctorRepositoryMock.Verify(x => x.Update(doctor), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
