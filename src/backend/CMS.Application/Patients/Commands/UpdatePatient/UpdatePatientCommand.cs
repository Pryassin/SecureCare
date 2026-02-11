using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Patients.Commands.UpdatePatient;

public record UpdatePatientCommand(
    Guid Id,
    string NationalId,
    string Phone,
    string? EmergencyContact) : IRequest<Result>;
