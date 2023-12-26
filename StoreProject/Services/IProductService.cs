using StoreProject.Models;

namespace StoreProject.Services;

public interface IProductService
{
    Task<bool> CreateProduct(Product? product);

    Task<IEnumerable<Product>> GetAllProducts();
    
    Task<IEnumerable<Product>> GetAllProductsWithCategories();

    Task<Product> GetProductById(int productId);

    Task<bool> UpdateProduct(Product? product);

    Task<bool> DeleteProduct(int productId);
}