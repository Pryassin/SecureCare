using CMS.Application.Appointments.Queries.GetAppointmentById;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Appointments.Queries;

public class GetAppointmentByIdQueryHandlerTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly GetAppointmentByIdQueryHandler _handler;

    public GetAppointmentByIdQueryHandlerTests()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _handler = new GetAppointmentByIdQueryHandler(_appointmentRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenAppointmentNotFound()
    {
        var query = new GetAppointmentByIdQuery(Guid.NewGuid());
        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenAppointmentExists()
    {
        var appointment = Appointment.Create(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1)).Value;
        var query = new GetAppointmentByIdQuery(appointment.Id);

        _appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(appointment.Id);
    }
}
