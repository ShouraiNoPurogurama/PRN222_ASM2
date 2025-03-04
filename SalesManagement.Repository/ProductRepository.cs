using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SalesManagement.Repositories.Models;
using SalesManagement.Repositories.Pagination;
using SalesManagement.Repository.Base;

namespace SalesManagement.Repository;

public class ProductRepository : GenericRepository<Product>
{
    public ProductRepository()
    {
    }

    /// <summary>
    /// Get all products asynchronously without pagination
    /// </summary>
    /// <returns>List of all <see cref="Product"/> entities in the database</returns>
    public new async Task<List<Product>> GetAllAsync()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return products;
    }

    /// <summary>
    /// Find matching products based on a condition 
    /// </summary>
    /// <param name="predicate">Condition to find</param>
    /// <returns>List of <see cref="Product"/> entities match the condition</returns>
    public async Task<(List<Product>,int)> FindByConditionAsync(Expression<Func<Product, bool>> predicate, PaginationRequest? paginationRequest)
    {
        var products = await _context.Products.Include(p => p.Category)
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(predicate)
            .Skip(paginationRequest?.PageIndex * paginationRequest?.PageSize ?? 0)
            .Take(paginationRequest?.PageSize ?? 5)
            .ToListAsync();

        var countWithoutPagination  = await _context.Products.Include(p => p.Category)
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(predicate)
            .CountAsync();
        
        return (products, countWithoutPagination);
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Equals(id));

        return product;
    }

    public long CountAll() => _context.Products.LongCount();
}