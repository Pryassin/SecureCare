using CMS.Domain.Common;
using CMS.Domain.Enums;
using MediatR;

namespace CMS.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password,
    UserRole Role) : IRequest<Result<Guid>>;
