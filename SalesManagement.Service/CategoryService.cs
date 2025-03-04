using SalesManagement.Repositories.Models;
using SalesManagement.Repository;

namespace SalesManagement.Service;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository = new ();

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }
}