using CMS.Application.Prescriptions.Queries.GetPrescriptionById;
using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CMS.Application.UnitTests.Prescriptions.Queries;

public class GetPrescriptionByIdQueryHandlerTests
{
    private readonly Mock<IPrescriptionRepository> _prescriptionRepositoryMock;
    private readonly GetPrescriptionByIdQueryHandler _handler;

    public GetPrescriptionByIdQueryHandlerTests()
    {
        _prescriptionRepositoryMock = new Mock<IPrescriptionRepository>();
        _handler = new GetPrescriptionByIdQueryHandler(_prescriptionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPrescriptionNotFound()
    {
        var query = new GetPrescriptionByIdQuery(Guid.NewGuid());
        _prescriptionRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Prescription?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenPrescriptionExists()
    {
        // Must use factory which might check validation
        var prescription = Prescription.Create(Guid.NewGuid(), "Meds", "Notes").Value;
        var query = new GetPrescriptionByIdQuery(prescription.Id);

        _prescriptionRepositoryMock.Setup(x => x.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(prescription);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.MedicationDetails.Should().Be(prescription.MedicationDetails);
    }
}
