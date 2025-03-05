using SalesManagement.Repositories.Models;

namespace SalesManagement.Service;

public class CategoryService
{

    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _unitOfWork.CategoryRepository.GetAllAsync();
    }
    
    public async Task<Category> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.CategoryRepository.GetByIdAsync(id);
    }
}