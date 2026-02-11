using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<Guid>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if Doctor is available
        // Note: In a production app, we would also check if the Patient and Doctor exist via their repositories.
        
        var isAvailable = await _appointmentRepository.IsDoctorAvailableAsync(request.DoctorId, request.DateTime, cancellationToken);
        if (!isAvailable)
        {
            return Result<Guid>.Failure(Errors.Appointment.Conflict);
        }

        var appointmentResult = Appointment.Create(
            request.PatientId, 
            request.DoctorId, 
            request.DateTime);

        if (appointmentResult.IsFailure)
        {
            return Result<Guid>.Failure(appointmentResult.Error);
        }

        _appointmentRepository.Add(appointmentResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return appointmentResult.Value.Id;
    }
}
