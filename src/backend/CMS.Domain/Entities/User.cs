using CMS.Domain.Common;
using CMS.Domain.Enums;
namespace CMS.Domain.Entities;

public  class User : BaseEntity
{
    public string Email { get; protected set; } = default!;
    public string PasswordHash { get; protected set; } = default!;
    public UserRole Role { get; protected set; }
    public bool IsActive { get; protected set; } = true;

    protected User() { }

    private User(Guid ID,string email, string passwordHash, UserRole role):base(ID)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public static Result<User>Create(string email, string passwordHash, UserRole role)
    {
        if(String.IsNullOrEmpty(email))
        {
              return Errors.User.EmailRequired;
        }
        if(String.IsNullOrEmpty(passwordHash))
        {
            return Errors.User.PasswordRequired;
        }
        if (!Enum.IsDefined(typeof(UserRole), role))
        {
             return Errors.User.InvalidRole; 
        }
        return new User(Guid.NewGuid(),email,passwordHash,role);
 
    }
    
    public Result Update(string email, UserRole role, bool isActive)
{
    // 1. Validation Logic
    if (string.IsNullOrWhiteSpace(email))
    {
        return Errors.User.EmailRequired;
    }

    if (!Enum.IsDefined(typeof(UserRole), role))
    {
        return Errors.User.InvalidRole;
    }

    Email = email;
    Role = role;
    IsActive = isActive;

    return Result.Success; 
}
 
 
}


