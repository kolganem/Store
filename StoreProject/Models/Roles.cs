namespace StoreProject.Models;

public static class Roles
{
    public const string Administrator = "Administrator";
    public const string ProfessionalUser = "ProfessionalUser";
    public const string User = "User";
}

public enum CustomUserRole
{
    Default = 0,
    Administrator,
    ProfessionalUser,
    User
}