using SalesManagement.Repositories.Models;
using SalesManagement.Repository;

namespace SalesManagement.Service;

public class UserAccountService
{
    private readonly UserAccountRepository _userAccountRepository = new();
    

    public async Task<UserAccount?> LoginAsync(string username, string password)
    {
        var user  = await _userAccountRepository.GetAsync(username, password);
        return user;
    }
}