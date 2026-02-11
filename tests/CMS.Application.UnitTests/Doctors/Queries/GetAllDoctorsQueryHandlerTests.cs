using CMS.Application.Doctors.Queries.GetAllDoctors;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Doctors.Queries;

public class GetAllDoctorsQueryHandlerTests
{
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly GetAllDoctorsQueryHandler _handler;

    public GetAllDoctorsQueryHandlerTests()
    {
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _handler = new GetAllDoctorsQueryHandler(_doctorRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllDoctors()
    {
        var d1 = Doctor.Create(Guid.NewGuid(), "1", "1").Value;
        var d2 = Doctor.Create(Guid.NewGuid(), "2", "2").Value;

        var list = new List<Doctor> { d1, d2 };

        _doctorRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var result = await _handler.Handle(new GetAllDoctorsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
}
