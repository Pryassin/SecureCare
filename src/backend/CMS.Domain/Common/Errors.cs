namespace CMS.Domain.Common;
public static class Errors
{
    public static class User
    {
        public static Error NotFound=>new("User.NotFound","User not found");
         public static Error InvalidEmail => new("User.InvalidEmail", "Invalid email format");
        public static Error AlreadyExists => new("User.AlreadyExists", "User already exists");
    }
    
}