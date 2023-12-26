using System.ComponentModel.DataAnnotations;
using StoreProject.Models;

namespace StoreProject.ViewModel;

public class RegisterViewModel
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public int EnteredRole { get; set; }
}