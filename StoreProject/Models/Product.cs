using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreProject.Models;

public class Product
{
    [Key] 
    public int Id { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }
    
    public string Name { get; set; }
    
    public string Info { get; set; }
    
    public double Price { get; set; }
    
    public string GeneralNote { get; set; }
    
    public string SpecialNote { get; set; }
}