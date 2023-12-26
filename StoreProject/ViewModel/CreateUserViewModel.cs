using System.ComponentModel.DataAnnotations;

namespace StoreProject.ViewModel;

public class CreateUserViewModel
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}