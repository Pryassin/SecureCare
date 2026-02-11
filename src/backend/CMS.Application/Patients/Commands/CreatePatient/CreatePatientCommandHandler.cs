using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Patients.Commands.CreatePatient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Guid>>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        // Check if patient already exists for this User
        var existingPatient = await _patientRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existingPatient is not null)
        {
            return Result<Guid>.Failure(new Error("Patient.AlreadyExists", "A patient profile already exists for this user."));
        }

        // Check if National ID is already taken
        var existingNationalId = await _patientRepository.GetByNationalIdAsync(request.NationalId, cancellationToken);
        if (existingNationalId is not null)
        {
            return Result<Guid>.Failure(new Error("Patient.DuplicateNationalId", "The provided National ID is already associated with another patient."));
        }

        var patientResult = Patient.Create(
            request.UserId,
            request.NationalId,
            request.DateOfBirth,
            request.Gender,
            request.Phone,
            request.EmergencyContact);

        if (patientResult.IsFailure)
        {
            return Result<Guid>.Failure(patientResult.Error);
        }

        _patientRepository.Add(patientResult.Value);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return patientResult.Value.Id;
    }
}
