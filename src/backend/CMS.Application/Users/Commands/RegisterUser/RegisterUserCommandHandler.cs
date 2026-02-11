using CMS.Domain.Common;
using CMS.Domain.Entities;
using CMS.Domain.Repositories;
using MediatR;
using BCrypt.Net;

namespace CMS.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingUser is not null)
        {
            return Result<Guid>.Failure(Errors.User.AlreadyExists);
        }

        // Hashing password using BCrypt
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var userResult = User.Create(
            request.Email,
            passwordHash,
            request.Role);

        if (userResult.IsFailure)
        {
            return Result<Guid>.Failure(userResult.Error);
        }

        _userRepository.Add(userResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return userResult.Value.Id;
    }
}
