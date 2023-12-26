using StoreProject.Models;

namespace StoreProject.ViewModel;

public class CreateProductViewModel
{
    public Product Product { get; set; }
    
    public IEnumerable<Category> PossibleCategories { get; set; }
    
    
}