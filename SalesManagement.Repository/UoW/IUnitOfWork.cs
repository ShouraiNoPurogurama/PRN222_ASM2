namespace SalesManagement.Repository.UoW;

public interface IUnitOfWork
{
    ProductRepository ProductRepository { get; }
    CategoryRepository CategoryRepository { get; }
    UserAccountRepository UserAccountRepository { get; }

    void SaveChanges();

    Task SaveChangesAsync();
}