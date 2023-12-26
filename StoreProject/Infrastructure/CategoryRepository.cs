using StoreProject.Models;

namespace StoreProject.Infrastructure;

public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(StoreDbContext dbContext) : base(dbContext)
    {

    }
}