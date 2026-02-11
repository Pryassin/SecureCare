using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using MediatR;

namespace CMS.Application.Doctors.Commands.CreateDoctor;

public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, Result<Guid>>
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDoctorCommandHandler(IDoctorRepository doctorRepository, IUnitOfWork unitOfWork)
    {
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var existingDoctor = await _doctorRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (existingDoctor is not null)
        {
            return Result<Guid>.Failure(new Error("Doctor.AlreadyExists", "A doctor profile already exists for this user."));
        }

        var doctorResult = Doctor.Create(
            request.UserId,
            request.LicenseNumber,
            request.Specialization);

        if (doctorResult.IsFailure)
        {
            return Result<Guid>.Failure(doctorResult.Error);
        }

        _doctorRepository.Add(doctorResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return doctorResult.Value.Id;
    }
}
