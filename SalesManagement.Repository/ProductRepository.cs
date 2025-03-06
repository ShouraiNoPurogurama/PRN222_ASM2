using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Repositories.Models;
using SalesManagement.Repository.Base;
using SalesManagement.Repository.Pagination;

namespace SalesManagement.Repository;

public class ProductRepository : GenericRepository<Product>
{
    public ProductRepository(SalesManagementDBContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Get all products asynchronously without pagination
    /// </summary>
    /// <returns>List of all <see cref="Product"/> entities in the database</returns>
    public async Task<PaginatedResult<Product>> GetAllAsync(PaginationRequest? paginationRequest)
    {
        var pageIndex = paginationRequest?.PageIndex ?? 0;

        var pageSize = paginationRequest?.PageSize ?? 5;

        var products = await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await _context.Products.LongCountAsync();

        return new PaginatedResult<Product>(pageIndex, pageSize, totalCount, products);
    }

    /// <summary>
    /// Find matching products based on a condition 
    /// </summary>
    /// <param name="predicate">Condition to find</param>
    /// <param name="paginationRequest">Pagination</param>
    /// <returns>List of <see cref="Product"/> entities match the condition</returns>
    public async Task<PaginatedResult<Product>> FindByConditionAsync(string? name, string? category,
        string? ingredients, PaginationRequest? paginationRequest)
    {
        var pageIndex = paginationRequest?.PageIndex ?? 0;

        var pageSize = paginationRequest?.PageSize ?? 5;

        var query = _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => (string.IsNullOrWhiteSpace(name) || p.Name.Contains(name)) &&
                        (string.IsNullOrWhiteSpace(category) || p.Category.Name.Contains(category)) &&
                        (string.IsNullOrWhiteSpace(ingredients) || p.Ingredients.Contains(ingredients)));

        var products = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await query.LongCountAsync();

        return new PaginatedResult<Product>(pageIndex, pageSize, totalCount, products);
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Equals(id));

        return product;
    }

    public async Task<int> UpdateAsync(Product product)
    {
        var existingProduct = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Equals(product.Id));

        if (string.IsNullOrEmpty(product.ImageFile))
        {
            product.ImageFile = existingProduct.ImageFile;
        }

        var category = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id.Equals(product.CategoryId));

        product.Category = category;

        _context.Products.Update(product);

        return await _context.SaveChangesAsync();
    }
}