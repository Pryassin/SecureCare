using CMS.Domain.Enums;
namespace CMS.Domain.Entities;

public abstract class User : BaseEntity
{
    public string Username { get; protected set; } = default!;
    public string PasswordHash { get; protected set; } = default!;
    public UserRole Role { get; protected set; }
    public bool IsActive { get; protected set; } = true;

    protected User() { }

    protected User(string username, string passwordHash, UserRole role)
    {
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }
}


