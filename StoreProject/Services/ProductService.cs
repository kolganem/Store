using StoreProject.Infrastructure;
using StoreProject.Models;

namespace StoreProject.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CreateProduct(Product? product)
    {
        if (product is not null)
        {
            await _unitOfWork.Products.Add(product);

            int result = _unitOfWork.Save();

            if (result > 0)
                return true;
            else
                return false;
        }
        return false;
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        if (productId > 0)
        {
            Product product = await _unitOfWork.Products.GetById(productId);
            
            if (product != null)
            {
                _unitOfWork.Products.Delete(product);
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

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        IEnumerable<Product> productList = await _unitOfWork.Products.GetAll();
        return productList;
    }

    public async Task<IEnumerable<Product>> GetAllProductsWithCategories()
    {
        IEnumerable<Product> productList = await _unitOfWork.Products.GetAllWithCategory();
        
        return productList;
    }

    public async Task<Product> GetProductById(int productId)
    {
        if (productId > 0)
        {
            Product productDetails = await _unitOfWork.Products.GetById(productId);
            
            if (productDetails != null)
            {
                return productDetails;
            }
        }
        return null;
    }

    public async Task<bool> UpdateProduct(Product? newProduct)
    {
        if (newProduct != null)
        {
            Product? product = await _unitOfWork.Products.GetById(newProduct.Id);
            
            if(product != null)
            {
                product.Name= newProduct.Name;
                product.Info= newProduct.Info;
                product.CategoryId = newProduct.CategoryId;
                product.Price= newProduct.Price;
                product.GeneralNote= newProduct.GeneralNote;
                product.SpecialNote= newProduct.SpecialNote;

                _unitOfWork.Products.Update(product);

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
