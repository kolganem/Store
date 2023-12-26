using StoreProject.Infrastructure;
using StoreProject.Models;

namespace StoreProject.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> CreateCategory(Category? category)
    {
        if (category is not null)
        {
            await _unitOfWork.Categories.Add(category);

            int result = _unitOfWork.Save();

            if (result > 0)
            {
                return true;
            }

            return false;
        }
        return false;
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        IEnumerable<Category> categories = await _unitOfWork.Categories.GetAll();
        return categories;
    }

    public async Task<Category?> GetCategoryById(int categoryId)
    {
        if (categoryId > 0)
        {
            Category? category = await _unitOfWork.Categories.GetById(categoryId);
            
            if (category != null)
            {
                return category;
            }
        }
        return null;
    }

    public async Task<bool> UpdateCategory(Category? newCategory)
    {
        if (newCategory != null)
        {
            Category? category = await _unitOfWork.Categories.GetById(newCategory.Id);
            
            if(category != null)
            {
                category.Id = newCategory.Id;
                category.Name = newCategory.Name;

                _unitOfWork.Categories.Update(category);

                int result = _unitOfWork.Save();

                if (result > 0)
                {
                    return true;
                }

                return false;
            }
        }
        return false;
    }

    public async Task<bool> DeleteCategory(int categoryId)
    {
        if (categoryId > 0)
        {
            Category category = await _unitOfWork.Categories.GetById(categoryId);
            
            if (category != null)
            {
                _unitOfWork.Categories.Delete(category);
                int result = _unitOfWork.Save();

                if (result > 0)
                {
                    return true;
                }

                return false;
            }
        }
        return false;
    }
    
}