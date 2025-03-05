using SalesManagement.Repository.Base;

namespace SalesManagement.Repository.UoW;

public class UnitOfWork(SalesManagementDBContext dbContext) : IUnitOfWork
{
    public GenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        return new GenericRepository<TEntity>(dbContext);
    }

    private ProductRepository? _productRepository;
    private CategoryRepository? _categoryRepository;
    private UserAccountRepository? _userAccountRepository;

    public ProductRepository ProductRepository => _productRepository ??= new ProductRepository(dbContext);

    public CategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(dbContext);
    
    public UserAccountRepository UserAccountRepository => _userAccountRepository ??= new UserAccountRepository(dbContext);
    
    public void SaveChanges()
    {
        dbContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}