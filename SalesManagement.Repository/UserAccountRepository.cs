using Microsoft.EntityFrameworkCore;
using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Base;

namespace SalesManagement.Repository;

public class UserAccountRepository : GenericRepository<UserAccount>
{
    public UserAccountRepository()
    {
    }

    public async Task<UserAccount?> GetAsync(string userName, string password)
    {
        return await _context.UserAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName.Equals(userName) && u.Password.Equals(password) && u.IsActive);
        
        // return await _context.UserAccounts
        //     .AsNoTracking()
        //     .FirstOrDefaultAsync(u => u.Phone.Equals(userName) && u.Password.Equals(password));
        
        // return await _context.UserAccounts
        //     .AsNoTracking()
        //     .FirstOrDefaultAsync(u => u.EmployeeCode.Equals(userName) && u.Password.Equals(password));
        
        // return await _context.UserAccounts
        //     .AsNoTracking()
        //     .FirstOrDefaultAsync(u => u.Email.Equals(userName) && u.Password.Equals(password));
    }
}