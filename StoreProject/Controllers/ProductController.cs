using Microsoft.AspNetCore.Mvc;
using StoreProject.Models;
using StoreProject.Services;
using StoreProject.ViewModel;

namespace StoreProject.Controllers;

[Route("[controller]/[action]")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public ProductController(IProductService productService, 
        ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        IEnumerable<Product> productDetailsList = await _productService.GetAllProductsWithCategories();
        
        if(productDetailsList == null)
        {
            return NotFound();
        }
        return View(productDetailsList);
    }
    
    public async Task<IActionResult> Details(int productId)
    {
        Product? productDetails = await _productService.GetProductById(productId);

        if (productDetails != null)
        {
            return View(productDetails);
        }

        return BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int productId)
    {
        Product product = new Product();
        
        if (productId != null)
        {
            if (ModelState.IsValid)
            {
                product = await _productService.GetProductById(productId);
            }
        }

        return View(product) ;
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Product product)
    {
        if (product != null)
        {
            bool isProductCreated = await _productService.UpdateProduct(product);
            if (isProductCreated)
            {
                return RedirectToAction(nameof(Index));
            }

            return BadRequest();

        }

        return BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        CreateProductViewModel createViewModel = new CreateProductViewModel()
        {
            Product = new Product(),
            PossibleCategories = await _categoryService.GetAllCategories()
        };
        return View(createViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        bool isProductCreated = await _productService.CreateProduct(product);

        if (isProductCreated)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest();
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int productId)
    {
        Product? productDetails = await _productService.GetProductById(productId);

        if (productDetails != null)
        {
            return View(productDetails);
        }

        return BadRequest();
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int productId)
    {
        bool isProductCreated = await _productService.DeleteProduct(productId);

        if (isProductCreated)
        {
            return RedirectToAction(nameof(Index));
        }

        return BadRequest();
    }
}
