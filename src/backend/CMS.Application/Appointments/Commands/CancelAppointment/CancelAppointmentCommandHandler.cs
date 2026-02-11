using CMS.Domain.Common;
using CMS.Domain.Enums;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Result>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.Id, cancellationToken);
        if (appointment is null)
        {
            return Result.Failure(new Error("Appointment.NotFound", "Appointment not found."));
        }

        var updateStatus = appointment.UpdateStatus(AppointmentStatus.Cancelled);
        if (updateStatus.IsFailure)
        {
            return updateStatus;
        }

        _appointmentRepository.Update(appointment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
