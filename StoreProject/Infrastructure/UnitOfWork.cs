namespace StoreProject.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly StoreDbContext _dbContext;
    public IProductRepository Products { get; }
    public ICategoryRepository Categories { get; }

    public UnitOfWork(StoreDbContext dbContext, IProductRepository productRepository,
        ICategoryRepository categories)
    {
        _dbContext = dbContext;
        Products = productRepository;
        Categories = categories;
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Dispose();
        }
    }

}