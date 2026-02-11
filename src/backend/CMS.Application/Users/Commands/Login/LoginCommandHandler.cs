using CMS.Application.Common.Interfaces;
using CMS.Domain.Common;
using CMS.Domain.Repositories;
using MediatR;
using BCrypt.Net;

namespace CMS.Application.Users.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure(new Error("Auth.InvalidCredentials", "Invalid email or password."));
        }

        // Verify password hash using BCrypt
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
             return Result<string>.Failure(new Error("Auth.InvalidCredentials", "Invalid email or password."));
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        return token;
    }
}
