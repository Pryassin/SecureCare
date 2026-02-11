using CMS.Domain.Common;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Prescriptions.Queries.GetPrescriptionById;

public class GetPrescriptionByIdQueryHandler : IRequestHandler<GetPrescriptionByIdQuery, Result<PrescriptionResponse>>
{
    private readonly IPrescriptionRepository _prescriptionRepository;

    public GetPrescriptionByIdQueryHandler(IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<Result<PrescriptionResponse>> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
    {
        var prescription = await _prescriptionRepository.GetByIdAsync(request.Id, cancellationToken);
        if (prescription is null)
        {
            return Result<PrescriptionResponse>.Failure(new Error("Prescription.NotFound", "Prescription not found."));
        }

        return new PrescriptionResponse(
            prescription.Id,
            prescription.AppointmentId,
            prescription.MedicationDetails,
            prescription.DoctorNotes,
            prescription.CreatedAt);
    }
}
