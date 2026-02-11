using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Enums;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Prescriptions.Commands.CreatePrescription;

public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, Result<Guid>>
{
    private readonly IPrescriptionRepository _prescriptionRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePrescriptionCommandHandler(
        IPrescriptionRepository prescriptionRepository, 
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _prescriptionRepository = prescriptionRepository;
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId, cancellationToken);
        
        if (appointment is null)
        {
            return Result<Guid>.Failure(new Error("Appointment.NotFound", "Appointment not found."));
        }

        // Business Rule: Can only create prescription for Completed appointments.
        // Assuming "Completed" is the state where doctor can add prescription, 
        // or maybe it's added during the visit which then marks it 'Completed'.
        // Let's assume the appointment must exist. The status verification can happen here.
        // Note: The PRD says "Doctors must be able to generate a Prescription record linked to a Completed appointment."
        // This implies the appointment should be in 'Completed' status.
        
        if (appointment.Status != AppointmentStatus.Completed)
        {
             return Result<Guid>.Failure(new Error("Prescription.InvalidStatus", "Prescriptions can only be added to completed appointments."));
        }

        var prescriptionResult = Prescription.Create(
            request.AppointmentId,
            request.MedicationDetails,
            request.DoctorNotes);

        if (prescriptionResult.IsFailure)
        {
            return Result<Guid>.Failure(prescriptionResult.Error);
        }

        _prescriptionRepository.Add(prescriptionResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return prescriptionResult.Value.Id;
    }
}
