using System.ComponentModel.DataAnnotations;

namespace StoreProject.Models;

public enum Categories
{
    Default = 0,
    
    [Display(Name = "Еда")]
    Food,
    [Display(Name = "Вкусности")]
    Goodies,
    [Display(Name = "Вода")]
    Water
}