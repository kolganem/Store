using StoreProject.Models;

namespace StoreProject.Services;

public interface ICategoryService
{
    Task<bool> CreateCategory(Category? product);

    Task<IEnumerable<Category>> GetAllCategories();

    Task<Category?> GetCategoryById(int productId);

    Task<bool> UpdateCategory(Category? product);

    Task<bool> DeleteCategory(int productId);
}