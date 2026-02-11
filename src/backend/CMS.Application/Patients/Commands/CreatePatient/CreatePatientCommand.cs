using CMS.Domain.Common;
using CMS.Domain.Enums;
using MediatR;

namespace CMS.Application.Patients.Commands.CreatePatient;

public record CreatePatientCommand(
    Guid UserId,
    string NationalId,
    DateTime DateOfBirth,
    Gender Gender,
    string Phone,
    string? EmergencyContact) : IRequest<Result<Guid>>;
