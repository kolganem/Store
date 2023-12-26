using Microsoft.EntityFrameworkCore;
using StoreProject.Models;

namespace StoreProject.Infrastructure;

public class ProductRepository: GenericRepository<Product>, IProductRepository
{
    public ProductRepository(StoreDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetAllWithCategory()
    {
        return await _dbContext.Products.Include(p => p.Category).ToListAsync();
    }
}