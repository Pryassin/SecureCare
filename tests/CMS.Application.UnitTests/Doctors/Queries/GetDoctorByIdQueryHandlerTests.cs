using CMS.Application.Doctors.Queries.GetDoctorById;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Doctors.Queries;

public class GetDoctorByIdQueryHandlerTests
{
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly GetDoctorByIdQueryHandler _handler;

    public GetDoctorByIdQueryHandlerTests()
    {
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _handler = new GetDoctorByIdQueryHandler(_doctorRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenDoctorNotFound()
    {
        var query = new GetDoctorByIdQuery(Guid.NewGuid());
        _doctorRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenDoctorExists()
    {
        var doctor = Doctor.Create(Guid.NewGuid(), "License", "Spec").Value;
        var query = new GetDoctorByIdQuery(doctor.Id);

        _doctorRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.LicenseNumber.Should().Be(doctor.LicenseNumber);
        result.Value.Specialization.Should().Be(doctor.Specialization);
    }
}
