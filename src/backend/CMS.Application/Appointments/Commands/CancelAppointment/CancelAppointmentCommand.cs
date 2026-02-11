using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Appointments.Commands.CancelAppointment;

public record CancelAppointmentCommand(Guid Id) : IRequest<Result>;
