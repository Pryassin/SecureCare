using CMS.Domain.Common;
using MediatR;

namespace CMS.Application.Users.Commands.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<Result<string>>;
