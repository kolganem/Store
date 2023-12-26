using StoreProject.Models;

namespace StoreProject.Infrastructure;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategory();
}