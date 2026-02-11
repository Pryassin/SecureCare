using CMS.Application.Patients.Queries.GetPatientById;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Patients.Queries;

public class GetPatientByIdQueryHandlerTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly GetPatientByIdQueryHandler _handler;

    public GetPatientByIdQueryHandlerTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _handler = new GetPatientByIdQueryHandler(_patientRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPatientNotFound()
    {
        var query = new GetPatientByIdQuery(Guid.NewGuid());
        _patientRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenPatientExists()
    {
        var patient = Patient.Create(Guid.NewGuid(), "123", DateTime.UtcNow, CMS.Domain.Enums.Gender.Female, "555", "Mom").Value;
        var query = new GetPatientByIdQuery(patient.Id);

        _patientRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.NationalId.Should().Be(patient.NationalId);
    }
}
