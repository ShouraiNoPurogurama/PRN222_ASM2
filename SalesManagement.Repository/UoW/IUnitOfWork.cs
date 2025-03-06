namespace SalesManagement.Repository.UoW;

public interface IUnitOfWork
{
    ProductRepository ProductRepository { get; }
    CategoryRepository CategoryRepository { get; }
    UserAccountRepository UserAccountRepository { get; }
    OutboxRepository OutboxRepository { get; }

    void SaveChanges();

    Task SaveChangesAsync();
}