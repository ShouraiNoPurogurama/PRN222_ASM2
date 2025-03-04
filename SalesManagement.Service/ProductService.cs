using SalesManagement.Repositories.Models;
using SalesManagement.Repositories.Pagination;
using SalesManagement.Repository;

namespace SalesManagement.Service;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<int> CreateAsync(Product product);
    Task<int> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
    Task<(IEnumerable<Product>, int)> Search(PaginationRequest paginationRequest, string? name, string? category, string? ingredients);
    long CountAll();
}

public class ProductService : IProductService
{
    private readonly ProductRepository _productRepository = new();

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(Product product)
    {
        return await _productRepository.CreateAsync(product);
    }

    public async Task<int> UpdateAsync(Product product)
    {
        return await _productRepository.UpdateAsync(product);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return await _productRepository.RemoveAsync(product);
    }

    public async Task<(IEnumerable<Product>, int)> Search(PaginationRequest paginationRequest, string? name, string? category, string? ingredients)
    {
        var productsAndCount = await _productRepository.FindByConditionAsync(p => 
            (string.IsNullOrEmpty(name) || p.Name.Contains(name)) &&
            (string.IsNullOrEmpty(category) || p.Category.Name.Contains(category)) &&
            (string.IsNullOrEmpty(ingredients) || p.Ingredients.Contains(ingredients)), paginationRequest);

        return productsAndCount;
    }

    public long CountAll() => _productRepository.CountAll();
}