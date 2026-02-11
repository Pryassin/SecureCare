using CMS.Domain.Common;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Doctors.Commands.UpdateDoctor;

public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Result>
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDoctorCommandHandler(IDoctorRepository doctorRepository, IUnitOfWork unitOfWork)
    {
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(request.Id, cancellationToken);
        if (doctor is null)
        {
            return Result.Failure(Errors.User.NotFound); // Or generic NotFound error
        }

        var updateResult = doctor.Update(request.LicenseNumber, request.Specialization);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        _doctorRepository.Update(doctor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
