using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreProject.Models;
using StoreProject.Services;

namespace StoreProject.Controllers;

[Authorize(Roles = Roles.ProfessionalUser + ","  + Roles.Administrator)]
[Route("[controller]/[action]")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> categories = await _categoryService.GetAllCategories();
        
        if(categories == null)
        {
            return NotFound();
        }
        return View(categories);
    }
    
    [HttpGet("{categoryId}")]
    public async Task<IActionResult> Details(int categoryId)
    {
        Category? category = await _categoryService.GetCategoryById(categoryId);

        if (category != null)
        {
            return View(category);
        }

        return BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int categoryId)
    {
        Category? product = new Category();
        
        if (categoryId != null)
        {
            if (ModelState.IsValid)
            {
                product = await _categoryService.GetCategoryById(categoryId);
            }
        }

        return View(product) ;
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Category category)
    {
        if (category != null)
        {
            bool isCategoryCreated = await _categoryService.UpdateCategory(category);
            if (isCategoryCreated)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }

        return BadRequest();
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        bool isCategoryCreated = await _categoryService.CreateCategory(category);

        if (isCategoryCreated)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int categoryId)
    {
        Category? category = await _categoryService.GetCategoryById(categoryId);

        if (category != null)
        {
            return View(category);
        }

        return BadRequest();
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int categoryId)
    {
        bool isCategoryDeleted = await _categoryService.DeleteCategory(categoryId);

        if (isCategoryDeleted)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest();
    }
}
