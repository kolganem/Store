namespace StoreProject.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }

    int Save();
}