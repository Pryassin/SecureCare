using CMS.Domain.Common;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Patients.Commands.UpdatePatient;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.Id, cancellationToken);
        if (patient is null)
        {
            return Result.Failure(Errors.User.NotFound); // Or specific Patient.NotFound
        }

        var updateResult = patient.Update(request.NationalId, request.Phone, request.EmergencyContact);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _patientRepository.Update(patient);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
