using Microsoft.AspNetCore.Identity;

namespace StoreProject.Models;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
}