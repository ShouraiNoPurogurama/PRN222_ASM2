using SalesManagement.Repositories.Models;
using SalesManagement.Repository;

namespace SalesManagement.Service;

public class UserAccountService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserAccountService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserAccount?> LoginAsync(string username, string password)
    {
        var user = await _unitOfWork.UserAccountRepository.GetAsync(username, password);
        return user;
    }
}