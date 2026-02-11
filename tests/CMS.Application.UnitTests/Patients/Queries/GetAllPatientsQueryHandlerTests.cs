using CMS.Application.Patients.Queries.GetAllPatients;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Patients.Queries;

public class GetAllPatientsQueryHandlerTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly GetAllPatientsQueryHandler _handler;

    public GetAllPatientsQueryHandlerTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _handler = new GetAllPatientsQueryHandler(_patientRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllPatients()
    {
        var patient1 = Patient.Create(Guid.NewGuid(), "1", DateTime.UtcNow, CMS.Domain.Enums.Gender.Male, "1", "1").Value;
        var patient2 = Patient.Create(Guid.NewGuid(), "2", DateTime.UtcNow, CMS.Domain.Enums.Gender.Male, "2", "2").Value;

        var list = new List<Patient> { patient1, patient2 };

        _patientRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var result = await _handler.Handle(new GetAllPatientsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
}
