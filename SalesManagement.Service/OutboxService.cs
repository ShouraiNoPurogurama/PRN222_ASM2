using SalesManagement.Repository.Models;

namespace SalesManagement.Service;

public class OutboxService
{
    private readonly IUnitOfWork _unitOfWork;

    public OutboxService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> CreateAsync(OutboxMessage outboxMessage)
    {
        return await _unitOfWork.OutboxRepository.CreateAsync(outboxMessage);
    }
}