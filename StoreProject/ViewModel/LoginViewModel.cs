using System.ComponentModel.DataAnnotations;

namespace StoreProject.ViewModel;

public class LoginViewModel
{
    [EmailAddress]
    public string Email { get; set; }

    public string Password { get; set; }
}