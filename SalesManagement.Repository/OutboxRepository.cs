using SalesManagement.Repository.Base;
using SalesManagement.Repository.Models;

namespace SalesManagement.Repository;

public class OutboxRepository : GenericRepository<OutboxMessage> {
    public OutboxRepository(SalesManagementDBContext dbContext) : base(dbContext)
    {
        
    }
    
    public async Task<int> CreateAsync(OutboxMessage outboxMessage)
    {
        await _context.OutboxMessages.AddAsync(outboxMessage);
        return await _context.SaveChangesAsync();
    }
}